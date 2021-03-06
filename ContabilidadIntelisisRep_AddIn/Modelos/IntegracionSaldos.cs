﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContabilidadIntelisisRep_AddIn.Modelos {

	public enum Tipos { Todos, Anticipos, SinAnticipos }
	public enum Empresas { Alianza, Embalatek, EmpNobleza, ExcelNobleza, Geometrika, Vanguardia, RoyalMile }

	public class IntegracionSaldos {
		//public static Empresas Empresa { get; set; } = Empresas.ExcelNobleza;

		public static string SqlCxp = @"Select Clase=Case When mt.Clave='CXP.A' Then 'Anticipos' Else 'Proveedores/Acreedores' End,Clave=Cxp.Proveedor,Nombre=Prov.Nombre,Categoria=Prov.Categoria,Grupo='cxp',
Familia=Prov.Familia,Cuenta=Prov.Cuenta,Tipo=Prov.Tipo,Mov=Cxp.Mov,MovId=Cxp.Movid,FechaEmision=Cxp.FechaEmision,Vencimiento=Cxp.Vencimiento,
Importe= isnull(Avg(cxp.Importe),0),Impuestos= isnull(avg(cxp.Impuestos),0),

Retencion= isnull(avg(cxp.Retencion2),0),
RetencionISR= isnull(avg( cxp.Retencion),0),

Saldo=Sum(Isnull(aux.Cargo,0)-Isnull(aux.Abono,0)),
Moneda=Aux.moneda,TipoCambio =Cxp.ProveedorTipoCambio,ImporteTotal=isnull(Avg(cxp.Importe),0)+ isnull(avg(cxp.Impuestos),0)-isnull(avg(cxp.Retencion),0),
Concepto= cxp.Concepto 
From Auxiliar aux Join MovTipo mt On aux.Aplica=mt.Mov And mt.Modulo='CXP' Join Cxp On Aux.Empresa=Cxp.Empresa And Cxp.Sucursal=aux.Sucursal And Cxp.Mov=aux.Aplica And Cxp.MovId=aux.AplicaId And Cxp.Estatus in ('CONCLUIDO','PENDIENTE') Join Prov On Cxp.Proveedor=Prov.Proveedor 
Where aux.Rama='CXP' And mt.Clave in ('CXP.F','CXP.NC','CXP.A','CXP.CAP','CXP.AJM','CXP.AJR','CXP.RA','CXP.FAC','CXP.DC') And mt.Mov<>'Retencion'
And Aux.Fecha<=@Fecha and ((@Tipo='Todos' ) or (@Tipo= 'Anticipos' and CxP.Mov='Anticipo') or (@Tipo= 'SinAnticipos' and CxP.Mov<>'Anticipo'))

Group by Cxp.Proveedor,Prov.Nombre,Prov.Categoria,Prov.Familia,Prov.Tipo,mt.Clave,Cxp.Mov,Cxp.MovId,Cxp.FechaEmision,Cxp.Vencimiento,Aux.Moneda,Cxp.ProveedorTipoCambio, Prov.Cuenta ,cxp.Concepto 
Having ((Sum(Isnull(aux.Cargo,0)-Isnull(aux.Abono,0)))< -0.01 or (Sum(Isnull(aux.Cargo,0)-Isnull(aux.Abono,0)))> 0.01)
Order by Aux.Moneda,1,Prov.Categoria,Prov.Familia,Prov.Nombre,Cxp.FechaEmision,Cxp.Mov,Cxp.MovId";

		public static string SqlCxc = @"
Select 
	Clase='N/A',
	Clave=Cxc.Cliente,
	Nombre=Cte.Nombre,
	Categoria=Cte.Categoria,
	Grupo=Cte.Grupo,
	Familia=Cte.Familia,
	Cuenta=Cte.Cuenta,
	Tipo=Cte.Tipo,
	Mov=Cxc.Mov,
	MovId=Cxc.Movid,
	FechaEmision=Cxc.FechaEmision,
	Vencimiento=Cxc.Vencimiento,
	Importe= isnull(Avg(cxc.Importe),0),
	Impuestos= isnull(avg(cxc.Impuestos),0),
	Retencion= isnull(avg(cxc.Retencion),0),
	Saldo=Sum(Isnull(aux.Cargo,0)-Isnull(aux.Abono,0)),
	Moneda=Aux.moneda,
	TipoCambio=Cxc.ClienteTipoCambio, 
	Concepto= cxc.Concepto
From Auxiliar aux 
	Join MovTipo mt On aux.Aplica=mt.Mov And mt.Modulo='CXC' 
	Join Cxc On aux.Empresa=Cxc.Empresa And aux.Sucursal=Cxc.Sucursal And aux.Aplica=Cxc.Mov And aux.AplicaId=Cxc.MovId And Cxc.Estatus in ('CONCLUIDO','PENDIENTE') 
	Join Cte On Cxc.Cliente=Cte.Cliente 
Where aux.Rama='CXC' And mt.Clave in ('CXC.F','CXC.FA','CXC.NC','CXC.A','CXC.DP','CXC.CD','CXC.CAP','CXC.AJM','CXC.AJR','CXC.RA','CXC.FAC','CXC.DC', 'CXC.DAC') And mt.Mov<>'Factura S/L' 
	And aux.Fecha<=@Fecha 
	and ((@Tipo='Todos') or (@Tipo= 'Anticipos' and Cxc.Mov in ('Anticipo','Anticipo Electronico','Anticipo CFDi', 'Canc Dev Sdo Cte' )) or (@Tipo= 'SinAnticipos' and Cxc.Mov not in ('Anticipo','Anticipo Electronico','Anticipo CFDi', 'Canc Dev Sdo Cte' )))

Group by Cxc.Cliente,Cte.Nombre,Cte.Categoria,Cte.Grupo,Cte.Familia,Cte.Tipo,mt.Clave,Cxc.Mov,Cxc.MovId,Cxc.FechaEmision,Cxc.Vencimiento,Aux.Moneda,Cxc.ClienteTipoCambio,Cte.Cuenta, cxc.Concepto
Having ((Sum(Isnull(aux.Cargo,0)-Isnull(aux.Abono,0)))< -0.01 or (Sum(Isnull(aux.Cargo,0)-Isnull(aux.Abono,0)))> 0.01) 
Order by Aux.Moneda,Cte.Categoria,Cte.Grupo,Cte.Familia,Cte.Nombre,Cxc.FechaEmision,Cxc.Mov,Cxc.MovId";

		public string Clase { get; set; }
		public string Clave { get; set; }
		public string Nombre { get; set; }
		public string Categoria { get; set; }
		public string Grupo { get; set; }
		public string Familia { get; set; }
		public string Cuenta { get; set; }
		public string Tipo { get; set; }
		public string Mov { get; set; }
		public string MovId { get; set; }
		public DateTime FechaEmision { get; set; }
		public DateTime Vencimiento { get; set; }
		public decimal Importe { get; set; }
		public decimal Impuestos { get; set; }
		public decimal Retencion { get; set; }
		public decimal? RetencionISR { get; set; } = 0;
		public decimal Saldo { get; set; }
		public string Moneda { get; set; }
		public double TipoCambio { get; set; }
		public string Concepto { get; set; }


		#region ReadonlyProperties

		public decimal ImporteTotal => Importe + Impuestos - Retencion - RetencionISR.Value;
		public decimal PorcIva { get { return Importe == 0 ? 0 : Impuestos / Importe; } }
		public decimal PorcRetIva { get { return Importe == 0 ? 0 : Retencion / Importe; } }
		public decimal PorcRetISR { get { return Importe == 0 || RetencionISR == null ? 0 : RetencionISR.Value / Importe; } }
		public decimal PorcImporte { get { return 1 + PorcIva - PorcRetIva - PorcRetISR; } }

		public decimal SaldoEnPesos { get { return ( decimal ) TipoCambio * Saldo; } }
		public decimal SaldoSubTotal { get { return SaldoEnPesos / PorcImporte; } }
		public decimal SaldoIva { get { return SaldoSubTotal * PorcIva; } }
		public decimal SaldoRetIva { get { return SaldoSubTotal * PorcRetIva * -1; } }
		public decimal SaldoRetISR => SaldoSubTotal * PorcRetISR * -1;
		public decimal SaldoTotal => SaldoSubTotal + SaldoIva + SaldoRetIva + SaldoRetISR;
		public decimal SaldoDiferencia { get { return SaldoEnPesos - SaldoTotal; } }

		#endregion



		public static async Task<List<IntegracionSaldos>> getCxcAsync ( DateTime Fecha , Tipos Tipo , Empresas Empresa ) {
			try {
				using ( var DB = new Context.dataBaseContext ( ) ) {

					var y = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder ( DB.Database.Connection.ConnectionString );
					y.Database = Empresa.ToString ( );
					DB.Database.Connection.ConnectionString = y.ConnectionString;
					DB.Database.CommandTimeout = 240;

					return await DB.Database.SqlQuery<IntegracionSaldos> ( SqlCxc , new SqlParameter ( "@Fecha" , Fecha ) , new SqlParameter ( "@Tipo" , Tipo.ToString ( ) ) ).ToListAsync ( );
				}
			} catch ( Exception ex ) {
				MessageBox.Show ( ex.Message );
				return null;
			}
		}


		public static async Task<List<IntegracionSaldos>> getCxpAsync ( DateTime Fecha , Tipos Tipo , Empresas Empresa ) {
			try {
				using ( var DB = new Context.dataBaseContext ( ) ) {

					var y = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder ( DB.Database.Connection.ConnectionString );
					y.Database = Empresa.ToString ( );
					DB.Database.Connection.ConnectionString = y.ConnectionString;
					DB.Database.CommandTimeout = 240;

					return await DB.Database.SqlQuery<IntegracionSaldos> ( SqlCxp , new SqlParameter ( "@Fecha" , Fecha ) , new SqlParameter ( "@Tipo" , Tipo.ToString ( ) ) ).ToListAsync ( );
				}
			} catch ( Exception ex ) {
				MessageBox.Show ( ex.Message );
				return null;
			}

		}
	}
}
