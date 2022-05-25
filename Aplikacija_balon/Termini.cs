using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Aplikacija_balon
{
    public partial class Termini : Form
    {
        int id = 0;
        DataTable termini;

        public Termini()
        {
            InitializeComponent();
        }

        private void Termini_Load(object sender, EventArgs e)
        {
            SqlConnection veza = Konekcija.Connect();            
            SqlDataAdapter adapter = new SqlDataAdapter("select email, ime, prezime, objekat.naziv from zaposleni join objekat on objekat.id = zaposleni.objekat_id where zaposleni.id = " + Program.id, veza);
            DataTable tabela = new DataTable();

            adapter.Fill(tabela);

            txt_email.Text = tabela.Rows[0]["email"].ToString();
            txt_ime.Text = tabela.Rows[0]["ime"].ToString();
            txt_prezime.Text = tabela.Rows[0]["prezime"].ToString();

            txt_objekat.Text = tabela.Rows[0]["naziv"].ToString();

            grid_popuni();
        }

        private void btn_dodaj_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection veza = Konekcija.Connect();

                SqlCommand komanda = new SqlCommand();
                komanda.Connection = veza;
                komanda.CommandType = CommandType.StoredProcedure;
                komanda.CommandText = "termin_dodaj";

                komanda.Parameters.Add(new SqlParameter("@datum", SqlDbType.Date, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dateTimePicker1.Value));
                komanda.Parameters.Add(new SqlParameter("@vreme", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, Convert.ToInt32(txt_vreme.Text)));
                komanda.Parameters.Add(new SqlParameter("@cena", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, Convert.ToInt32(txt_cena.Text)));
                komanda.Parameters.Add(new SqlParameter("@objekat_id", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, Program.objekat_id));
                komanda.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, true, 0, 0, "", DataRowVersion.Current, null));

                veza.Open();
                komanda.ExecuteNonQuery();
                veza.Close();

                int ret;
                ret = (int) komanda.Parameters["@RETURN_VALUE"].Value;
                if (ret == -1)
                {
                    MessageBox.Show("Termin je vec unet!");
                }
                else
                {
                    grid_popuni();
                }
            }
            catch (Exception greska)
            {
                MessageBox.Show(greska.Message);
            }
        }

        private void grid_popuni()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select id, datum, vreme, cena from termini where objekat_id = " + Program.objekat_id, Konekcija.Connect());
            termini = new DataTable();
            adapter.Fill(termini);
            dataGridView1.DataSource = termini;
            dataGridView1.Columns["id"].Visible = false;
        }

        private void btn_obrisi_Click(object sender, EventArgs e)
        {
            SqlConnection veza = Konekcija.Connect();
            SqlCommand komanda = new SqlCommand("DELETE FROM termini where id = " + id, veza);

            try
            {
                veza.Open();
                komanda.ExecuteNonQuery();
                veza.Close();

                grid_popuni();
            }
            catch (Exception greska)
            {
                MessageBox.Show(greska.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                postavi(e.RowIndex);
                id = (int)termini.Rows[e.RowIndex]["id"];
            }
        }

        private void postavi(int broj_sloga)
        {
            
            dateTimePicker1.Value = (DateTime) termini.Rows[broj_sloga]["datum"];
            txt_vreme.Text = termini.Rows[broj_sloga]["vreme"].ToString();
            txt_cena.Text = termini.Rows[broj_sloga]["cena"].ToString();            
        }
    }
}
