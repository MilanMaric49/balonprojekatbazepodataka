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
    public partial class Zaposleni : Form
    {
        int id = 0;
        DataTable rezervacije;

        public Zaposleni()
        {
            InitializeComponent();
        }

        private void Zaposleni_Load(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select * from zaposleni where id = " + Program.id, Konekcija.Connect());
            DataTable tabela = new DataTable();
            adapter.Fill(tabela);
            txt_email.Text = tabela.Rows[0]["email"].ToString();
            txt_ime.Text = tabela.Rows[0]["ime"].ToString();
            txt_prezime.Text = tabela.Rows[0]["prezime"].ToString();

            adapter = new SqlDataAdapter("select * from korisnik", Konekcija.Connect());
            tabela = new DataTable();
            adapter.Fill(tabela);
            cmb_korisnik.DataSource = tabela;
            cmb_korisnik.ValueMember = "id";
            cmb_korisnik.DisplayMember = "email";
            cmb_korisnik.SelectedIndex = -1;

            adapter = new SqlDataAdapter("select * from objekat", Konekcija.Connect());
            tabela = new DataTable();
            adapter.Fill(tabela);
            cmb_objekat.DataSource = tabela;
            cmb_objekat.ValueMember = "id";
            cmb_objekat.DisplayMember = "naziv";
            cmb_objekat.SelectedIndex = -1;

            grid_popuni();
        }

        private void grid_popuni()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select rezervacija.id, korisnik.email, rezervacija.korisnik_id, objekat.naziv, rezervacija.objekat_id, datum, pocetak, kraj from rezervacija join korisnik on korisnik.id = rezervacija.korisnik_id join objekat on objekat.id = rezervacija.objekat_id", Konekcija.Connect());
            rezervacije = new DataTable();
            adapter.Fill(rezervacije);
            dataGridView1.DataSource = rezervacije;
            dataGridView1.Columns["korisnik_id"].Visible = false;
            dataGridView1.Columns["objekat_id"].Visible = false;
            dataGridView1.Columns["id"].Visible = false;
        }

        private void Zaposleni_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void cmb_objekat_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmb_objekat.IsHandleCreated && cmb_objekat.Focused)
            {
                SqlDataAdapter adapter = new SqlDataAdapter("select * from termini where objekat_id = " + cmb_objekat.SelectedValue + " and datum = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'", Konekcija.Connect());
                DataTable tabela = new DataTable();
                adapter.Fill(tabela);

                cmb_pocetak.DataSource = tabela.Copy();
                cmb_pocetak.DisplayMember = "vreme";
                cmb_pocetak.SelectedIndex = -1;

                cmb_kraj.DataSource = tabela.Copy();
                cmb_kraj.DisplayMember = "vreme";
                cmb_kraj.SelectedIndex = -1;
            }
        }

        private void cmb_pocetak_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmb_pocetak.IsHandleCreated && cmb_pocetak.Focused)
            {
                try
                {
                    SqlConnection veza = Konekcija.Connect();
                    SqlCommand komanda = new SqlCommand("select cena from termini where objekat_id = " + cmb_objekat.SelectedValue + " and datum = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' AND vreme = " + cmb_pocetak.Text, veza);

                    veza.Open();
                    txt_posatu.Text = komanda.ExecuteScalar().ToString();
                    veza.Close();
                }
                catch (Exception greska)
                {
                    MessageBox.Show(greska.Message);
                }
            }
        }        

        private void btn_dodaj_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection veza = Konekcija.Connect();

                SqlCommand komanda = new SqlCommand();
                komanda.Connection = veza;
                komanda.CommandType = CommandType.StoredProcedure;
                komanda.CommandText = "rezervacija_dodaj";

                komanda.Parameters.Add(new SqlParameter("@korisnik_id", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, cmb_korisnik.SelectedValue));
                komanda.Parameters.Add(new SqlParameter("@objekat_id", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, cmb_objekat.SelectedValue));
                komanda.Parameters.Add(new SqlParameter("@datum", SqlDbType.Date, 100, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, dateTimePicker1.Value));
                komanda.Parameters.Add(new SqlParameter("@pocetak", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, Convert.ToInt32(cmb_pocetak.Text)));
                komanda.Parameters.Add(new SqlParameter("@kraj", SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Current, Convert.ToInt32(cmb_kraj.Text)));
                komanda.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, true, 0, 0, "", DataRowVersion.Current, null));

                veza.Open();
                komanda.ExecuteNonQuery();
                veza.Close();

                int ret;
                ret = (int)komanda.Parameters["@RETURN_VALUE"].Value;
                if (ret == -1)
                {
                    MessageBox.Show("Postoji rezervacija koja se preklapa sa unetom!");
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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select * from termini where objekat_id = " + cmb_objekat.SelectedValue + " and datum = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'", Konekcija.Connect());
            DataTable tabela = new DataTable();
            adapter.Fill(tabela);

            cmb_pocetak.DataSource = tabela.Copy();
            cmb_pocetak.DisplayMember = "vreme";
            cmb_pocetak.SelectedIndex = -1;

            cmb_kraj.DataSource = tabela.Copy();
            cmb_kraj.DisplayMember = "vreme";
            cmb_kraj.SelectedIndex = -1;
        }

        private void btn_obrisi_Click(object sender, EventArgs e)
        {
            SqlConnection veza = Konekcija.Connect();
            SqlCommand komanda = new SqlCommand("DELETE FROM rezervacija where id = " + id, veza);

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
                id = (int) rezervacije.Rows[e.RowIndex]["id"];
            }
        }

        private void postavi(int broj_sloga)
        {
            cmb_korisnik.SelectedValue = rezervacije.Rows[broj_sloga]["korisnik_id"];
            cmb_objekat.SelectedValue = rezervacije.Rows[broj_sloga]["objekat_id"];
            dateTimePicker1.Value = (DateTime) rezervacije.Rows[broj_sloga]["datum"];
            cmb_pocetak.Text = rezervacije.Rows[broj_sloga]["pocetak"].ToString();
            cmb_kraj.Text = rezervacije.Rows[broj_sloga]["kraj"].ToString();

            try
            {
                SqlConnection veza = Konekcija.Connect();
                SqlCommand komanda = new SqlCommand("select cena from termini where objekat_id = " + cmb_objekat.SelectedValue + " and datum = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' AND vreme = " + cmb_pocetak.Text, veza);                
                veza.Open();
                txt_posatu.Text = komanda.ExecuteScalar().ToString();
                veza.Close();
            }
            catch (Exception greska)
            {
                MessageBox.Show(greska.Message);
            }

            if (Convert.ToInt32(cmb_kraj.Text) - Convert.ToInt32(cmb_pocetak.Text) <= 0)
            {
                MessageBox.Show("Unesite odgovarajuca vremena!");
            }
            else
            {
                if (Convert.ToInt32(cmb_kraj.Text) - Convert.ToInt32(cmb_pocetak.Text) >= 4)
                {
                    MessageBox.Show("Ne mozete rezervisati na toliko vremena!");
                }
                else
                {
                    txt_brojsati.Text = Convert.ToString(Convert.ToInt32(cmb_kraj.Text) - Convert.ToInt32(cmb_pocetak.Text));
                    txt_ukupno.Text = Convert.ToString(Convert.ToInt32(txt_posatu.Text) * Convert.ToInt32(txt_brojsati.Text));
                }
            }
        }

        private void cmb_kraj_TextChanged(object sender, EventArgs e)
        {
            if (cmb_kraj.IsHandleCreated && cmb_kraj.Focused)
            {
                if (Convert.ToInt32(cmb_kraj.Text) - Convert.ToInt32(cmb_pocetak.Text) <= 0)
                {
                    MessageBox.Show("Unesite odgovarajuca vremena!");
                }
                else
                {
                    if (Convert.ToInt32(cmb_kraj.Text) - Convert.ToInt32(cmb_pocetak.Text) >= 4)
                    {
                        MessageBox.Show("Ne mozete rezervisati na toliko vremena!");
                    }
                    else
                    {
                        txt_brojsati.Text = Convert.ToString(Convert.ToInt32(cmb_kraj.Text) - Convert.ToInt32(cmb_pocetak.Text));
                        txt_ukupno.Text = Convert.ToString(Convert.ToInt32(txt_posatu.Text) * Convert.ToInt32(txt_brojsati.Text));
                    }
                }
            }
        }

        private void прегледТерминаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Termini termini = new Termini();
            termini.Show();
        }
    }
}
