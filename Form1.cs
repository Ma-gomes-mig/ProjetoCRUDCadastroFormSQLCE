using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using System.IO;

namespace ProjetoCRUDCadastroFormSQLCe
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Método para coneção com o banco de dados, poderia colocar global.
        private void btnConect_Click(object sender, EventArgs e)
        {
            //String definindo o caminho da base de dados
            string baseDados = Application.StartupPath + @"\Db\DBSQLServer.sdf";
            //Definição da stringConnection
            string strConection = @"DataSource = " + baseDados + "; Password = '1234'";
            //Criação do objeto SQLServerEngine
            SqlCeEngine Db = new SqlCeEngine(strConection);
            //Verificação da existencia da base de dados, caso não exista é criada
            if(!File.Exists(baseDados)) 
            {
                Db.CreateDatabase();
            }
            //Fechamento do obj
            Db.Dispose();

            //Conexão Criada
            SqlCeConnection conexao = new SqlCeConnection(strConection);
            try
            {
                //Abrir a conexão
                conexao.Open();
                //Caso a conexão seja criada a msg é lançada
                labelResultado.Text = "Conectado com SQL Server CE";
            }
            catch (Exception ex)
            {
                //Caso não a msg de erro é lançada
                labelResultado.Text = "Erro ao conextar ao base de dados, por favor verifique sua senha ou se a bse existe. \n" + ex;
            }
            finally
            {
                conexao.Close();
            }               
        }

        //Método para criar a tabela 
        private void btnCreateTab_Click(object sender, EventArgs e)
        {            
            string baseDados = Application.StartupPath + @"\Db\DBSQLServer.sdf";
            string strConection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection connection = new SqlCeConnection(strConection);

            try
            {
                connection.Open();

                SqlCeCommand command = new SqlCeCommand();
                command.Connection = connection;

                command.CommandText = "CREATE TABLE Pessoa (id INT NOT NULL PRIMARY KEY, nome NVARCHAR(50)," +
                    " email NVARCHAR(70), cpf NVARCHAR(20), telefone NVARCHAR(20))";
                command.ExecuteNonQuery();

                labelResultado.Text = "Tabela Criada SQL Server CE";
                command.Dispose();
            }
            catch(Exception ex)
            {
                labelResultado.Text = ex.Message;
            }
            finally
            {
                connection.Close();
            }
        }


        //CREAT = C
        private void btnInsert_Click(object sender, EventArgs e)
        {
            string baseDados = Application.StartupPath + @"\Db\DBSQLServer.sdf";
            string strConection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection connection = new SqlCeConnection(strConection);

            try
            {
                connection.Open();

                SqlCeCommand command = new SqlCeCommand();
                command.Connection = connection;

                int id = new Random(DateTime.Now.Millisecond).Next(0,10000);
                string nome = textBoxNome.Text;
                string email = textBoxMail.Text;
                string cpf = maskedTextCpf.Text;
                string tel = maskedTextTel.Text;

                command.CommandText = "INSERT INTO Pessoa VALUES (" + id + ", ' " + nome + "' , '" + email + "','" + cpf +"','" + tel + "')";
                textBoxNome.Clear();
                textBoxMail.Clear();
                maskedTextCpf.Clear();
                maskedTextTel.Clear();                
                command.ExecuteNonQuery(); 
                

                labelResultado.Text = "Pessoa inserida com sucesso";
                
                command.Dispose();
            }
            catch (Exception ex)
            {
                labelResultado.Text = ex.Message;
            }
            finally
            {
                connection.Close();
            }
        }

        //READ = R
        private void btnSearch_Click(object sender, EventArgs e)
        {
            labelResultado.Text = "";
            dataGridView.Rows.Clear();

            string baseDados = Application.StartupPath + @"\Db\DBSQLServer.sdf";
            string strConection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection connection = new SqlCeConnection(strConection);

            try
            {
                string query = "SELECT * FROM Pessoa";

                if(textBoxNome.Text != "")
                {
                    query = "SELECT * FROM Pessoa WHERE nome LIKE '" + textBoxNome.Text + "'";
                }

                DataTable dados = new DataTable();

                SqlCeDataAdapter adapter = new SqlCeDataAdapter(query, strConection);

                connection.Open();

                adapter.Fill(dados);

                foreach(DataRow row in dados.Rows) 
                {
                    dataGridView.Rows.Add(row.ItemArray);
                }

                   
            }
            catch (Exception ex)
            {
                dataGridView.Rows.Clear();
                labelResultado.Text = ex.Message;
            }
            finally
            {
                connection.Close();
            }
        }

        //UPDATE = U
        private void btnEdit_Click(object sender, EventArgs e)
        {
            string baseDados = Application.StartupPath + @"\Db\DBSQLServer.sdf";
            string strConection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection connection = new SqlCeConnection(strConection);

            try
            {
                connection.Open();

                SqlCeCommand command = new SqlCeCommand();
                command.Connection = connection;

                int id = (int)dataGridView.SelectedRows[0].Cells[0].Value;
                string nome = textBoxNome.Text;
                string email = textBoxMail.Text;
                string cpf = maskedTextCpf.Text;
                string telefone = maskedTextTel.Text;

                string query = "UPDATE Pessoa SET nome = '" + nome + "', email = '" + email + "', cpf = '" + cpf + "', telefone = '" + telefone + "' WHERE id LIKE '" + id + "'";

                command.CommandText = query;
                textBoxNome.Clear();
                textBoxMail.Clear();
                maskedTextCpf.Clear();
                maskedTextTel.Clear();                
                command.ExecuteNonQuery();


                labelResultado.Text = "Registro Alterado com sucesso";

                command.Dispose();
            }
            catch (Exception ex)
            {
                labelResultado.Text = ex.Message;
            }
            finally
            {
                connection.Close();
            }
        }

        //DELETE = D
        private void btnExclui_Click(object sender, EventArgs e)
        {
            string baseDados = Application.StartupPath + @"\Db\DBSQLServer.sdf";
            string strConection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection connection = new SqlCeConnection(strConection);

            try
            {
                connection.Open();

                SqlCeCommand command = new SqlCeCommand();
                command.Connection = connection;

                int id = (int)dataGridView.SelectedRows[0].Cells[0].Value;

                command.CommandText = "DELETE FROM Pessoa WHERE id = '"+ id + "'";
                textBoxNome.Clear();
                textBoxMail.Clear();
                maskedTextCpf.Clear();
                maskedTextTel.Clear();
                command.ExecuteNonQuery();


                labelResultado.Text = "Registro Excluido com sucesso";

                command.Dispose();
            }
            catch (Exception ex)
            {
                labelResultado.Text = ex.Message;
            }
            finally
            {
                connection.Close();
            }
        }

        
    }
}
