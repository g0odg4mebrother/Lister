using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Dapper;

namespace ListOFDapper
{
    public partial class Form1 : Form
    {
        private SqlConnection connection;
        private string connectionString;
        private DataTable dataTable;

        public Form1()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["ListOFDapper.Properties.Settings.stringBD"]?.ConnectionString;
            InitializeDataGridView();
            InitializeControls();
        }

        private void InitializeDataGridView()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
        }

        private void InitializeControls()
        {
           
            var connectionGroup = new GroupBox()
            {
                Text = "Управление подключением",
                Location = new Point(10, 10),
                Size = new Size(300, 80)
            };

            connect.Location = new Point(10, 20);
            disconect.Location = new Point(100, 20);
            vivodInformation.Location = new Point(190, 20);

            connectionGroup.Controls.AddRange(new Control[] { connect, disconect, vivodInformation });
            this.Controls.Add(connectionGroup);

           
            var mainQueriesGroup = new GroupBox()
            {
                Text = "Основные запросы",
                Location = new Point(10, 100),
                Size = new Size(300, 180)
            };

            Imail.Location = new Point(10, 20);
            List.Location = new Point(10, 50);
            Auksion.Location = new Point(10, 80);
            City.Location = new Point(10, 110);
            Country.Location = new Point(10, 140);

            mainQueriesGroup.Controls.AddRange(new Control[] { Imail, List, Auksion, City, Country });
            this.Controls.Add(mainQueriesGroup);

            
            var filterGroup = new GroupBox()
            {
                Text = "Фильтрация данных",
                Location = new Point(320, 10),
                Size = new Size(300, 180)
            };

            OpredCity.Location = new Point(10, 20);
            opredCountry.Location = new Point(10, 50);
            acsiiCountry.Location = new Point(10, 80);
            PokVCity.Location = new Point(10, 110);
            VCountry.Location = new Point(10, 140);

            filterGroup.Controls.AddRange(new Control[] { OpredCity, opredCountry, acsiiCountry, PokVCity, VCountry });
            this.Controls.Add(filterGroup);

            
            var statsGroup = new GroupBox()
            {
                Text = "Статистика",
                Location = new Point(630, 10),
                Size = new Size(300, 180)
            };

            button2.Location = new Point(10, 20);
            button3.Location = new Point(10, 50);
            button6.Location = new Point(10, 80);
            button7.Location = new Point(10, 110);
            button8.Location = new Point(150, 20);
            button9.Location = new Point(150, 50);

            statsGroup.Controls.AddRange(new Control[] { button2, button3, button6, button7, button8, button9 });
            this.Controls.Add(statsGroup);

         
            var customQueriesGroup = new GroupBox()
            {
                Text = "Пользовательские запросы",
                Location = new Point(10, 290),
                Size = new Size(300, 120)
            };

            button1.Location = new Point(10, 20);
            button4.Location = new Point(10, 50);
            button5.Location = new Point(10, 80);

            customQueriesGroup.Controls.AddRange(new Control[] { button1, button4, button5 });
            this.Controls.Add(customQueriesGroup);

       
            var dataOperationsGroup = new GroupBox()
            {
                Text = "Операции с данными",
                Location = new Point(320, 200),
                Size = new Size(300, 210)
            };

            
            var insertPanel = new Panel() { Location = new Point(10, 20), Size = new Size(280, 50) };
            var insertLabel = new Label() { Text = "Данные для вставки:", Location = new Point(0, 0), AutoSize = true };
            textBox1.Location = new Point(0, 20);
            textBox1.Size = new Size(200, 20);
            vstavka.Location = new Point(210, 20);
            vstavka.Size = new Size(60, 20);
            insertPanel.Controls.AddRange(new Control[] { insertLabel, textBox1, vstavka });

          
            var updatePanel = new Panel() { Location = new Point(10, 80), Size = new Size(280, 50) };
            var updateLabel = new Label() { Text = "Обновление имени:", Location = new Point(0, 0), AutoSize = true };
            textBox2.Location = new Point(0, 20);
            textBox2.Size = new Size(90, 20);
            var updateLabel2 = new Label() { Text = "→", Location = new Point(95, 22), AutoSize = true };
            textBox3.Location = new Point(110, 20);
            textBox3.Size = new Size(90, 20);
            obnova.Location = new Point(210, 20);
            obnova.Size = new Size(60, 20);
            updatePanel.Controls.AddRange(new Control[] { updateLabel, textBox2, updateLabel2, textBox3, obnova });

        
            var deletePanel = new Panel() { Location = new Point(10, 140), Size = new Size(280, 60) };
            var deleteLabel = new Label() { Text = "Удаление по значению:", Location = new Point(0, 0), AutoSize = true };
            textBox4.Location = new Point(0, 20);
            textBox4.Size = new Size(120, 20);
            delite.Location = new Point(125, 20);
            delite.Size = new Size(45, 20);
            delContry.Location = new Point(175, 20);
            delContry.Size = new Size(45, 20);
            delCity.Location = new Point(125, 45);
            delCity.Size = new Size(45, 20);
            DelSection.Location = new Point(175, 45);
            DelSection.Size = new Size(45, 20);
            delAuksionTovar.Location = new Point(225, 20);
            delAuksionTovar.Size = new Size(45, 45);
            deletePanel.Controls.AddRange(new Control[] { deleteLabel, textBox4, delite, delContry, delCity, DelSection, delAuksionTovar });

            dataOperationsGroup.Controls.AddRange(new Control[] { insertPanel, updatePanel, deletePanel });
            this.Controls.Add(dataOperationsGroup);

        
            var concreteQueriesGroup = new GroupBox()
            {
                Text = "Конкретные запросы",
                Location = new Point(630, 200),
                Size = new Size(300, 120)
            };

            var concretePanel = new Panel() { Location = new Point(10, 20), Size = new Size(280, 90) };
            var concreteLabel = new Label() { Text = "Значение для поиска:", Location = new Point(0, 0), AutoSize = true };
            textBox5.Location = new Point(0, 20);
            textBox5.Size = new Size(120, 20);
            cityConcrete.Location = new Point(125, 20);
            cityConcrete.Size = new Size(70, 20);
            ConcretePerson.Location = new Point(125, 45);
            ConcretePerson.Size = new Size(70, 20);
            ConcretSection.Location = new Point(125, 70);
            ConcretSection.Size = new Size(70, 20);
            concretePanel.Controls.AddRange(new Control[] { concreteLabel, textBox5, cityConcrete, ConcretePerson, ConcretSection });
            concreteQueriesGroup.Controls.Add(concretePanel);
            this.Controls.Add(concreteQueriesGroup);

           
            dataGridView1.Location = new Point(10, 420);
            dataGridView1.Size = new Size(920, 250);

       
            this.Size = new Size(950, 700);
            this.Text = "Система управления рассылкой - База данных покупателей";
        }

        private void connect_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    MessageBox.Show("Строка подключения не найдена в конфигурации.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                connection = new SqlConnection(connectionString);
                connection.Open();
                MessageBox.Show("Подключение к базе данных установлено успешно.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateConnectionStatus(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateConnectionStatus(false);
            }
        }

        private void disconect_Click(object sender, EventArgs e)
        {
            if (connection?.State == ConnectionState.Open)
            {
                connection.Close();
                connection.Dispose();
                connection = null;
                MessageBox.Show("Подключение к базе данных закрыто.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateConnectionStatus(false);
            }
            else
            {
                MessageBox.Show("Нет активного подключения к базе данных.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateConnectionStatus(bool isConnected)
        {
            this.Text = $"Система управления рассылкой - База данных покупателей - {(isConnected ? "Подключено" : "Отключено")}";
        }

        private async void ShowQueryResults(string query, object parameters = null)
        {
            if (connection?.State != ConnectionState.Open)
            {
                MessageBox.Show("Сначала установите соединение с базой данных.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var result = await connection.QueryAsync(query, parameters);
                
                if (result == null || !result.Any())
                {
                    dataGridView1.DataSource = null;
                    MessageBox.Show("Запрос не вернул данных.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                dataTable = new DataTable();
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

                dataGridView1.DataSource = dataTable;
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выполнения запроса: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void vivodInformation_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT * FROM List");
        }

        private void Imail_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT Email FROM List");
        }

        private void List_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT DISTINCT Section FROM List");
        }

        private void Auksion_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT DISTINCT AuksionTovar FROM List");
        }

        private void City_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT DISTINCT City FROM List");
        }

        private void Country_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT DISTINCT Country FROM List");
        }


        private void OpredCity_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT * FROM List WHERE City = 'Novgorod'");
        }

        private void opredCountry_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT * FROM List WHERE Country = 'Russia'");
        }

        private void acsiiCountry_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT AuksionTovar, Acsia FROM List WHERE Country = 'Russia'");
        }

        private void PokVCity_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT City, COUNT(*) AS NumberOfBuyers FROM List GROUP BY City");
        }

        private void VCountry_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT Country, COUNT(*) AS CountOfCustomers FROM List GROUP BY Country");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT Country, COUNT(DISTINCT City) AS NumberOfCities FROM List GROUP BY Country");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT AVG(CAST(NumberOfCities AS FLOAT)) AS AverageCitiesPerCountry FROM (SELECT COUNT(DISTINCT City) AS NumberOfCities FROM List GROUP BY Country) AS Subquery");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT TOP 3 Country, COUNT(*) AS NumberOfBuyers FROM List GROUP BY Country ORDER BY NumberOfBuyers DESC");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT TOP 1 Country, COUNT(*) AS NumberOfBuyers FROM List GROUP BY Country ORDER BY NumberOfBuyers DESC");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT TOP 3 City, COUNT(*) AS NumberOfBuyers FROM List GROUP BY City ORDER BY NumberOfBuyers DESC");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT TOP 1 City, COUNT(*) AS NumberOfBuyers FROM List GROUP BY City ORDER BY NumberOfBuyers DESC");
        }


        private void button1_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT Section FROM List WHERE Country = 'Russia' AND LastName = 'Михалыя'");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT AuksionTovar FROM List WHERE Section = 'мобилки' AND StartOfShares = '2010-09-21' AND EndOfShares = '2011-12-31'");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ShowQueryResults("SELECT AuksionTovar FROM List WHERE LastName = 'Михалыя'");
        }


        private void vstavka_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Введите данные для вставки.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var inputFields = textBox1.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                
                if (inputFields.Length != 15)
                {
                    MessageBox.Show("Введите все 15 полей через запятую.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var parameters = new
                {
                    ID = int.Parse(inputFields[0].Trim()),
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

                var insertQuery = @"INSERT INTO List (ID, Firstname, Lastname, PatrName, DateOfBirth, Gender, Email, Country, City, Section, AuksionTovar, StartOfShares, EndOfShares, Acsia) 
                                  VALUES (@ID, @Firstname, @Lastname, @PatrName, @DateOfBirth, @Gender, @Email, @Country, @City, @Section, @AuksionTovar, @StartOfShares, @EndOfShares, @Acsia)";

                var affectedRows = connection.Execute(insertQuery, parameters);
                MessageBox.Show($"Добавлено {affectedRows} записей.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при вставке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void obnova_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Введите старое и новое имя.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var parameters = new { oldName = textBox2.Text.Trim(), newName = textBox3.Text.Trim() };
                var updateQuery = "UPDATE List SET FirstName = @newName WHERE FirstName = @oldName";
                
                var affectedRows = connection.Execute(updateQuery, parameters);
                MessageBox.Show(affectedRows > 0 ? $"Обновлено {affectedRows} записей." : "Имя не найдено.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                textBox2.Clear();
                textBox3.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void delite_Click(object sender, EventArgs e)
        {
            ExecuteDeleteOperation("FirstName", textBox4.Text.Trim(), "покупатель(ей)");
        }

        private void delContry_Click(object sender, EventArgs e)
        {
            ExecuteDeleteOperation("Country", textBox4.Text.Trim(), "страны(стран)");
        }

        private void delCity_Click(object sender, EventArgs e)
        {
            ExecuteDeleteOperation("City", textBox4.Text.Trim(), "город(а)");
        }

        private void DelSection_Click(object sender, EventArgs e)
        {
            ExecuteDeleteOperation("Section", textBox4.Text.Trim(), "раздел(ов)");
        }

        private void delAuksionTovar_Click(object sender, EventArgs e)
        {
            ExecuteDeleteOperation("AuksionTovar", textBox4.Text.Trim(), "товар(ов) акции");
        }

        private void ExecuteDeleteOperation(string columnName, string value, string messageText)
        {
            if (string.IsNullOrEmpty(value))
            {
                MessageBox.Show($"Введите значение для {columnName}.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var deleteQuery = $"DELETE FROM List WHERE {columnName} = @value";
                var parameters = new { value };

                var deletedRows = connection.Execute(deleteQuery, parameters);
                MessageBox.Show(deletedRows > 0 ? $"Удалено {deletedRows} {messageText}." : "Записи не найдены.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                textBox4.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void cityConcrete_Click(object sender, EventArgs e)
        {
            ExecuteSelectOperation("DISTINCT City", "Country", textBox5.Text.Trim(), "Города в стране");
        }

        private void ConcretePerson_Click(object sender, EventArgs e)
        {
            ExecuteSelectOperation("DISTINCT Section", "FirstName", textBox5.Text.Trim(), "Разделы покупателя");
        }

        private void ConcretSection_Click(object sender, EventArgs e)
        {
            ExecuteSelectOperation("DISTINCT AuksionTovar", "Section", textBox5.Text.Trim(), "Акционные товары раздела");
        }

        private void ExecuteSelectOperation(string selectColumn, string whereColumn, string value, string messagePrefix)
        {
            if (string.IsNullOrEmpty(value))
            {
                MessageBox.Show($"Введите значение для {whereColumn}.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var selectQuery = $"SELECT {selectColumn} FROM List WHERE {whereColumn} = @value";
                var parameters = new { value };

                var results = connection.Query<string>(selectQuery, parameters).ToList();

                if (results.Count > 0)
                {
                    ShowQueryResults(selectQuery, parameters);
                }
                else
                {
                    MessageBox.Show("Данные не найдены.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении запроса: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            connection?.Close();
            connection?.Dispose();
            base.OnFormClosing(e);
        }
    }
}

