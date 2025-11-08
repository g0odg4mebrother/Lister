using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Dapper;

namespace ListOFDapper
{
    public partial class Form1 : Form
    {
        private SqlConnection connection;
        private string connectionString;

        public Form1()
        {
            connectionString = ConfigurationManager.ConnectionStrings["ListOFDapper.Properties.Settings.stringBD"].ConnectionString;
            InitializeComponent();
        }

        private void vivodInformation_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT * FROM List");
        }

        private void disconect_Click(object sender, EventArgs e)
        {
            if (connection?.State == ConnectionState.Open)
            {
                connection.Close();
                MessageBox.Show("Подключение закрыто.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Нет активного соединения.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void connect_Click(object sender, EventArgs e)
        {
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                MessageBox.Show("Подключено успешно.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ShowQueryResults(string query)
        {
            if (connection?.State != ConnectionState.Open)
            {
                MessageBox.Show("Сначала установите соединение.", "Предупреждение");
                return;
            }

            try
            {
                var result = await connection.QueryAsync(query);
                var dataTable = new DataTable();

                MessageBox.Show($"Результатов: {result.Count()}", "Отладка");

                if (result.Any())
                {
                    var firstRow = (IDictionary<string, object>)result.First();
                    foreach (var column in firstRow.Keys)
                    {
                        dataTable.Columns.Add(column);
                    }

                    foreach (var row in result)
                    {
                        var rowData = (IDictionary<string, object>)row;
                        var newRow = dataTable.NewRow();
                        foreach (var cell in rowData)
                        {
                            newRow[cell.Key] = cell.Value ?? DBNull.Value;
                        }
                        dataTable.Rows.Add(newRow);
                    }
                }

                dataGridView1.DataSource = dataTable;
                MessageBox.Show("Данные загружены");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void Imail_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT Email FROM List");
        }

        private void List_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT Section FROM List");
        }

        private void Auksion_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT AuksionTovar FROM List");
        }

        private void City_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT City FROM List");
        }

        private void Country_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT Country FROM List");
        }

        private void OpredCity_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT City,LastName FROM List WHERE City='Novgorod'");
        }

        private void opredCountry_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT Country,LastName FROM List WHERE Country='Russia'");
        }

        private void acsiiCountry_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT Country,Acsia FROM List WHERE Country='Russia'");
        }

        private void PokVCity_Click(object sender, EventArgs e)
        {
            ShowQueryResults(@"SELECT City, COUNT(*) AS NumberOfBuyers FROM List GROUP BY City");
        }

        private void VCountry_Click(object sender, EventArgs e)
        {
            ShowQueryResults(@"SELECT Country, COUNT(*) AS CountOfCustomers FROM List GROUP BY Country");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowQueryResults(@"SELECT Country, COUNT(DISTINCT City) AS NumberOfCities FROM List GROUP BY Country");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ShowQueryResults(@"SELECT AVG(NumberOfCities) AS AverageCitiesPerCountry FROM (
                SELECT COUNT(DISTINCT City) AS NumberOfCities FROM List GROUP BY Country
            ) AS Subquery");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowQueryResults(@"SELECT Section FROM List WHERE Country = 'Russia' AND LastName = 'Михалыя'");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ShowQueryResults(@"SELECT AuksionTovar FROM List 
                WHERE Section = 'мобилки' AND StartOfShares = '2010-09-21' AND EndOfShares = '2011-12-31'");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ShowQueryResults(@"SELECT AuksionTovar FROM List WHERE LastName = 'Михалыя'");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ShowQueryResults(@"SELECT TOP 3 Country, COUNT(*) AS NumberOfBuyers 
                FROM List GROUP BY Country ORDER BY NumberOfBuyers DESC");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ShowQueryResults(@"SELECT TOP 1 Country, COUNT(*) AS NumberOfBuyers 
                FROM List GROUP BY Country ORDER BY NumberOfBuyers DESC");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ShowQueryResults(@"SELECT TOP 3 City, COUNT(*) AS NumberOfBuyers 
                FROM List GROUP BY City ORDER BY NumberOfBuyers DESC");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ShowQueryResults(@"SELECT TOP 1 City, COUNT(*) AS NumberOfBuyers 
                FROM List GROUP BY City ORDER BY NumberOfBuyers DESC");
        }

        private void vstavka_Click(object sender, EventArgs e)
        {
            var inputFields = textBox1.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (inputFields.Length != 15)
            {
                MessageBox.Show("Пожалуйста, введите все 15 полей через запятую: ID и 14 остальных");
                return;
            }

            if (!int.TryParse(inputFields[0].Trim(), out int id))
            {
                MessageBox.Show("Некорректный ID");
                return;
            }

            try
            {
                var parameters = new
                {
                    ID = id,
                    Firstname = inputFields[1].Trim(),
                    Lastname = inputFields[2].Trim(),
                    PatrName = inputFields[3].Trim(),
                    DateOfBirth = DateTime.Parse(inputFields[4].Trim()),
                    Gender = inputFields[5].Trim(),
                    Email = inputFields[6].Trim(),
                    Country = inputFields[7].Trim(),
                    City = inputFields[8].Trim(),
                    Section = inputFields[9].Trim(),
                    AuksionTovar = inputFields[10].Trim(),
                    StartOfShares = DateTime.Parse(inputFields[11].Trim()),
                    EndOfShares = DateTime.Parse(inputFields[12].Trim()),
                    Acsia = int.Parse(inputFields[13].Trim())
                };

                using (var conn = new SqlConnection(connectionString))
                {
                    var insertQuery = @"INSERT INTO YourTableName 
                        (ID, Firstname, Lastname, PatrName, DateOfBirth, Gender, Email, Country, City, Section, AuksionTovar, StartOfShares, EndOfShares, Acsia)
                        VALUES (@ID, @Firstname, @Lastname, @PatrName, @DateOfBirth, @Gender, @Email, @Country, @City, @Section, @AuksionTovar, @StartOfShares, @EndOfShares, @Acsia)";

                    conn.Execute(insertQuery, parameters);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при вставке данных: {ex.Message}");
            }
        }

        private void obnova_Click(object sender, EventArgs e)
        {
            var oldName = textBox1.Text.Trim();
            var newName = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(oldName) || string.IsNullOrEmpty(newName))
            {
                MessageBox.Show("Пожалуйста, введите существующее и новое имя");
                return;
            }

            try
            {
                var updateQuery = "UPDATE List SET FirstName = @newName WHERE FirstName = @oldName";
                var parameters = new { oldName, newName };

                var affectedRows = connection.Execute(updateQuery, parameters);
                MessageBox.Show(affectedRows > 0 ? $"Обновлено {affectedRows} записей" : "Имя не найдено в таблице");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
            }
        }

        private void delite_Click(object sender, EventArgs e)
        {
            ExecuteDeleteOperation("FirstName", textBox3.Text.Trim(), "покупатель(ей)");
        }

        private void delContry_Click(object sender, EventArgs e)
        {
            ExecuteDeleteOperation("Country", textBox3.Text.Trim(), "страны(стран)");
        }

        private void delCity_Click(object sender, EventArgs e)
        {
            ExecuteDeleteOperation("City", textBox3.Text.Trim(), "город(а)");
        }

        private void DelSection_Click(object sender, EventArgs e)
        {
            ExecuteDeleteOperation("Section", textBox3.Text.Trim(), "раздел(ов)");
        }

        private void delAuksionTovar_Click(object sender, EventArgs e)
        {
            ExecuteDeleteOperation("AuksionTovar", textBox3.Text.Trim(), "товар(ов) акции");
        }

        private void ExecuteDeleteOperation(string columnName, string value, string messageText)
        {
            if (string.IsNullOrEmpty(value))
            {
                MessageBox.Show($"Пожалуйста, введите значение для {columnName}");
                return;
            }

            try
            {
                var deleteQuery = $"DELETE FROM List WHERE {columnName} = @value";
                var parameters = new { value };

                var deletedRows = connection.Execute(deleteQuery, parameters);
                MessageBox.Show(deletedRows > 0 ? $"Удалено {deletedRows} {messageText}" : "Записи не найдены");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении удаления: {ex.Message}");
            }
        }

        private void cityConcrete_Click(object sender, EventArgs e)
        {
            ExecuteSelectOperation("City", "Country", textBox4.Text.Trim(), "Города в стране");
        }

        private void ConcretePerson_Click(object sender, EventArgs e)
        {
            ExecuteSelectOperation("Section", "FirstName", textBox4.Text.Trim(), "Разделы покупателя");
        }

        private void ConcretSection_Click(object sender, EventArgs e)
        {
            ExecuteSelectOperation("AuksionTovar", "Section", textBox4.Text.Trim(), "Акционные товары раздела");
        }

        private void ExecuteSelectOperation(string selectColumn, string whereColumn, string value, string messagePrefix)
        {
            if (string.IsNullOrEmpty(value))
            {
                MessageBox.Show($"Пожалуйста, введите значение для {whereColumn}");
                return;
            }

            try
            {
                var selectQuery = $"SELECT {selectColumn} FROM List WHERE {whereColumn} = @value";
                var parameters = new { value };

                var results = connection.Query<string>(selectQuery, parameters).ToList();

                if (results.Count > 0)
                {
                    MessageBox.Show($"{messagePrefix} {value}:\n{string.Join(", ", results)}");
                }
                else
                {
                    MessageBox.Show("Данные не найдены");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении запроса: {ex.Message}");
            }
        }
    }
}