﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using libProduccionDataBase.Contexto;
using libProduccionDataBase.Tablas;


namespace ExcelNoblezaControlProduccion.DockContents.Catalogos
{
    public partial class CatalogoMateriales : BaseForm.BaseFormCatalogos
    {
        public CatalogoMateriales( DataBaseContexto DB ) : base( DB )
        {
            InitializeComponent();         
            this.Load += Catalogo_Load;
        }

        /// <summary>
        /// Carga Asincronica del Formulario, muestra la imagen del preloader si es necesario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Catalogo_Load( object sender, EventArgs e )
        {
            LoaderPicktureBox.Visible = true;
            InfoContainerkryptonSplitContainer.Visible = false;
            OnStatusStringChanged( new ChangeStatusMessageEventArgs { Title = "Cargando..", Message = "Cargando Materiales..." } );


                if (!Program.IsChangingTheme)await DB.FamiliasMateriales.Include( mt => mt.Materiales ).LoadAsync();

            familiaMaterialesBindingSource.DataSource = this.DB.FamiliasMateriales.Local.ToBindingList();
            InfoContainerkryptonSplitContainer.Visible = true;
            LoaderPicktureBox.Visible = false;
            OnStatusStringChanged( new ChangeStatusMessageEventArgs { Title = "Listo", Message = "Listo" } );
        }

        /// <summary>
        /// Abre el formulario de editar un Item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Editar( object sender, EventArgs e )
        {

            var frm= new CatalogosAddEdit.AgregarEditarMaterial(this, (Material) materialesBindingSource.Current);

            MainForm mainfrm= this.DockPanel.FindForm() as MainForm;
            frm.StatusStringChanged += mainfrm.CambioEstadoFormCatalog;

            frm.Show( this.DockPanel );
        }

        /// <summary>
        /// Abre el formulario para agregar un Item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Agregar( object sender, EventArgs e )
        {

            var frm= new CatalogosAddEdit.AgregarEditarMaterial(this);
           
            MainForm mainfrm= this.DockPanel.FindForm() as MainForm;
            frm.StatusStringChanged += mainfrm.CambioEstadoFormCatalog;

            frm.Show( this.DockPanel );
        }

        /// <summary>
        /// Elimina un Item de la coleccion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Eliminar( object sender, EventArgs e )
        {
            try
            {
                if (materialesBindingSource.Current != null && MessagesActionsAndForms.AskConfirmation( this ))
                {
                    this.materialesBindingSource.RemoveCurrent();

                    foreach(var mt in DB.Materiales.Local.ToList())
                    {
                        if(mt.FamiliaMateriales== null)
                        {
                            DB.Materiales.Remove( mt );
                        }
                    }
                   
                    DB.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show( libProduccionDataBase.Auxiliares.GetInnerException( ex ) );
            }
        }

        /// <summary>
        /// Refresca los elementos de la coleccion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async override void Actualizar( object sender, EventArgs e )
        {
            LoaderPicktureBox.Visible = true;
            InfoContainerkryptonSplitContainer.Visible = false;
            OnStatusStringChanged( new ChangeStatusMessageEventArgs { Title = "Cargando..", Message = "Cargando Clientes..." } );

            //await Task.Run( () =>
            //{

            //    ((System.Data.Entity.Infrastructure.IObjectContextAdapter)this.DB)
            //   .ObjectContext
            //   .Refresh( System.Data.Entity.Core.Objects.RefreshMode.StoreWins, DB.FamiliasMateriales );


            //    ((System.Data.Entity.Infrastructure.IObjectContextAdapter)this.DB)
            //   .ObjectContext
            //   .Refresh( System.Data.Entity.Core.Objects.RefreshMode.StoreWins, DB.Materiales );
            //} );
            
            await DB.FamiliasMateriales.Include(t=> t.Materiales).LoadAsync();

            familiaMaterialesBindingSource.DataSource = DB.FamiliasMateriales.Local;
            familiaMaterialesBindingSource.ResetBindings( false );
            FamiliaMaterialesKryptonListBox.Invalidate();
            materialesBindingSource.ResetBindings( false );
            materialesKryptonListBox.Invalidate();

            LoaderPicktureBox.Visible = false;
            InfoContainerkryptonSplitContainer.Visible = true;
            OnStatusStringChanged( new ChangeStatusMessageEventArgs { Title = "Listo", Message = "Listo" } );
        }

        /// <summary>
        /// Filtra la coleccion y busca lo elementos que contengal un valor parecido al string
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="SearchString"></param>
        public override void Buscar( object sender, string SearchString )
        {
            // Filtro de entidades hijo.

            if (SearchString.Trim() == string.Empty) { familiaMaterialesBindingSource.DataSource = DB.FamiliasMateriales.Local; return; }

            var _q= DB.Materiales.Include(nameof(Material.FamiliaMateriales))
                .Where(i=> i.Apariencia.ToLower().Contains(SearchString.ToLower()))
                .Select(_item=> new { _item, _item.FamiliaMateriales} );//.Apariencia.ToLower().Contains(SearchString.ToLower())

            var Fam= from _fam in _q.ToArray()
                     group _fam
                     by _fam.FamiliaMateriales.Id
                     into _coll
                     select FamiliaMateriales.AttachMateriales(_coll.First().FamiliaMateriales, _coll.Select(a=>a._item).ToList()); //_os.First().FamiliaMateriales.AttachMateriales(_os.Select(a=>a._item).ToList()) ;// _os.First().FamiliaMateriales.AttachMateriales(_os.Select(a=>a._item).ToList());

            familiaMaterialesBindingSource.DataSource = Fam;

        }

    }
}