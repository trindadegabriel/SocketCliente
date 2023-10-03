using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace Socket_Cliente
{
    public partial class FrmCliente : Form
    {
        private string NomeUsuario;
        private StreamWriter stwEnviador;
        private StreamReader strReceptor;
        private TcpClient tcpServidor;
        private delegate void AtualizaLogCallBack(string Mensagem);
        private delegate void FechaConexaoCallBack(string Motivo);
        private Thread mensagemThread;
        private IPAddress enderecoIP;
        private bool Conectado;
        private int enderecoPorta;
        private int porta;
        
        public FrmCliente()
        {
            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            InitializeComponent();
        }
        
        private void ReceberMensagem()
        {
            strReceptor = new StreamReader(tcpServidor.GetStream()); 
            string ConResposta = strReceptor.ReadLine();
              if (ConResposta[0] == '1')
              {
                this.Invoke(new AtualizaLogCallBack(this.AtualizaLog), new object[] { " Conectado com sucesso! " });
              }
              else
              {
                string Motivo = "Não conectado:";
                Motivo += ConResposta;
                this.Invoke(new FechaConexaoCallBack(this.FechaConexao), new object[] { Motivo });
                
                return;
              }
            while (Conectado)
            {   
                try
                {
                    this.Invoke(new AtualizaLogCallBack(this.AtualizaLog), new object[] { strReceptor.ReadLine() });
                }
                catch
                { 
                }
            }
        }

        private void AtualizaLog(string strMensagem)
        {
            txtMsgRecebida.AppendText(strMensagem + "\r\n");
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            if(txtNome.Text =="" || txtIp.Text =="" || txtPorta.Text =="")
            {
                MessageBox.Show("Preencha todos os campos.");
                txtNome.Focus();
                return;
            }
            try
            {
                porta = VerificaCaracteresValidos(txtPorta.Text);
            }
            catch (Exception)
            {
                throw;
            }
            if(Conectado == false && porta != 0)
            {
                InicializaConexao();
                label4.Visible = true;
                txtMsgEnviada.Visible = true;
                btnEnviar.Visible = true;
                label5.Visible = true;
                txtMsgRecebida.Visible = true;
                btnDesconectar.Visible = true;
            }
            else
            {
                return;
            }
        }

        private int VerificaCaracteresValidos(string text)
        {
            bool converte = int.TryParse(text, out int numero);
            if (converte)
            {
                return numero;
            }
            else
            {
                MessageBox.Show("Insira apenas números na porta.");
                txtPorta.Text = "";
            }
            return 0;
        }

        private void InicializaConexao()
        {
            enderecoIP = IPAddress.Parse(txtIp.Text);
            enderecoPorta = Convert.ToInt32(txtPorta.Text);
            tcpServidor = new TcpClient();
            tcpServidor.Connect(enderecoIP, enderecoPorta);

            Conectado = true;

            NomeUsuario = txtNome.Text;
            txtIp.Enabled = true;
            txtNome.Enabled = true;
            txtMsgEnviada.Enabled = true;
            btnEnviar.Enabled = true;
            btnConectar.Enabled = false;
            btnDesconectar.Enabled = true;

            stwEnviador = new StreamWriter(tcpServidor.GetStream());
            stwEnviador.WriteLine(txtNome.Text);
            stwEnviador.Flush();

            mensagemThread = new Thread(new ThreadStart(ReceberMensagem));
            mensagemThread.Start();
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            EnviarMensagem();
        }

        private void EnviarMensagem()
        {
            if(txtMsgEnviada.Lines.Length >= 1)
            {
                stwEnviador.WriteLine(txtMsgEnviada.Text);
                stwEnviador.Flush();
                txtMsgEnviada.Lines = null;
            }
            txtMsgEnviada.Text = "";
        }

        private void FechaConexao(string Motivo)
        {
            txtMsgRecebida.AppendText(Motivo);
            txtIp.Enabled = true;
            txtNome.Enabled = true;
            txtMsgEnviada.Enabled = false;
            btnEnviar.Enabled = false;
            btnConectar.Text = "Conectado";

            Conectado = false;
            stwEnviador.Close();
            strReceptor.Close();
            tcpServidor.Close();
        }

        public void OnApplicationExit(object sender, EventArgs e)
        {
            if (Conectado == true)
            {
                Conectado = false;
                stwEnviador.Close();
                strReceptor.Close();
                tcpServidor.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FechaConexao("Desconectado");
        }
    }
}
