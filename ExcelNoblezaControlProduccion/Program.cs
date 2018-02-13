﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelNoblezaControlProduccion
{
    static class Program
    {
        public static bool IsChangingTheme = false;

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
            Application.Run( new MainForm() );

            Properties.Settings.Default.Save();
        }
    }
}