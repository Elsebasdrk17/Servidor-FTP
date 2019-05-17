using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace ftpApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            // desabilitamos el boton en lo que el proceso de la funcion es realizado
            btnEnviar.Enabled = false;
            //procesa los mensajes en la cola
            Application.DoEvents();
            //una vez procesado se manda la funcion encargada de subir el archivo al servidor
            subirArchivo(txtRutaFTP.Text, txtRutaArchivo.Text, txtUsuario.Text, txtContra.Text);
            //una vez terminado el proceso volvemos habilitar el boton
            btnEnviar.Enabled = true;
            //es algo asi como no te dejare apretar el boton hasta que termine el proceso
        }
        private void btnArchivo_Click(object sender, EventArgs e)
        {
            // Si todo esta bien, poner la ruta del archivo en la caja de texto
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtRutaArchivo.Text = openFileDialog1.FileName;
            }
        }

        private void txtRutaFTP_Leave(object sender, EventArgs e)
        {
            /* si el texto no inicia con [ftp://] agregarlo al inicio de la caja de texto 
             * para mantener el formato al salir de la caja de texto
             */
            if (!txtRutaFTP.Text.StartsWith("ftp://"))
            {
                txtRutaFTP.Text = "ftp://" + txtRutaFTP.Text;
            }
        }
        private void subirArchivo(string rutaFTP, string rutaArchivo, string usuario, string contraseña)
        {
            /*creamos un objeto consulta de tipo ftpwebrequest es algo asi como la herencia que hicimos con los botones de elevador
             *luego hacemos un evento create y lo llenamos con el formato correcto
             */ 
            FtpWebRequest consulta = (FtpWebRequest)FtpWebRequest.Create(rutaFTP + "/" + Path.GetFileName(rutaArchivo));
            //el tipo de metodo que se utilizara
            consulta.Method = WebRequestMethods.Ftp.UploadFile;
            //crear credenciales para la conexion
            consulta.Credentials = new NetworkCredential(usuario, contraseña);
            //habilitar o deshabilitar ciertas funciones
            consulta.UsePassive = true;
            consulta.UseBinary = true;
            consulta.KeepAlive = false;
            /*cargar el archivo seleccionado*/
            FileStream cargar = File.OpenRead(rutaArchivo);
            //creamos un buffer para guardar los datos
            byte[] buffer = new byte[cargar.Length];
            cargar.Read(buffer, 0, buffer.Length);
            cargar.Close();
            /*enviar el archivo*/
            // el objeto ftpwebrequest lo hacemos un stream para poder utilizar la funcion del stream con los datos obtenidos
            Stream streamQuery = consulta.GetRequestStream();
            //escribinos en el buffer
            streamQuery.Write(buffer, 0, buffer.Length);
            streamQuery.Close();
            //una vez terminado el proceso le mostramos al usuario si se subio el archivo o no
            MessageBox.Show("Archivo subido exitosamente");
        }
    }
}
