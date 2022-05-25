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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void dugme_login_Click(object sender, EventArgs e)
        {
            if (txt_email.Text == "" || txt_lozinka.Text == "")
            {
                MessageBox.Show("Niste uneli podatke.");
                return;
            }
            else
            {
                try
                {
                    SqlConnection veza = Konekcija.Connect();
                    SqlCommand komanda = new SqlCommand("SELECT * FROM Korisnik WHERE email = @username", veza);
                    komanda.Parameters.AddWithValue("@username", txt_email.Text);
                    SqlDataAdapter adapter = new SqlDataAdapter(komanda);
                    DataTable tabela = new DataTable();
                    adapter.Fill(tabela);

                    int brojac = tabela.Rows.Count;
                    if (brojac == 1)
                    {
                        if (String.Compare(tabela.Rows[0]["lozinka"].ToString(), txt_lozinka.Text) == 0)
                        {
                            MessageBox.Show("Uspesno logovanje");
                            Program.id = (int) tabela.Rows[0]["id"];

                            this.Hide();
                            
                            Korisnik frm_korisnik = new Korisnik();
                            frm_korisnik.Show();
                        }
                        else
                        {
                            MessageBox.Show("Pogresna lozinka!");
                        }
                    }
                    else
                    {
                        veza = Konekcija.Connect();
                        komanda = new SqlCommand("SELECT * FROM Zaposleni WHERE email = @username", veza);
                        komanda.Parameters.AddWithValue("@username", txt_email.Text);
                        adapter = new SqlDataAdapter(komanda);
                        tabela = new DataTable();
                        adapter.Fill(tabela);

                        brojac = tabela.Rows.Count;
                        if (brojac == 1)
                        {
                            if (String.Compare(tabela.Rows[0]["lozinka"].ToString(), txt_lozinka.Text) == 0)
                            {
                                MessageBox.Show("Uspesno logovanje");
                                Program.id = (int) tabela.Rows[0]["id"];
                                Program.objekat_id = (int)tabela.Rows[0]["objekat_id"];
                                this.Hide();

                                Zaposleni frm_zaposleni = new Zaposleni();
                                frm_zaposleni.Show();
                            }
                            else
                            {
                                MessageBox.Show("Pogresna lozinka!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Uneli ste nepostojecu imejl adresu!");
                        }
                    }
                }
                catch (Exception greska)
                {
                    MessageBox.Show(greska.Message);
                }
            }
        }
    }
}
