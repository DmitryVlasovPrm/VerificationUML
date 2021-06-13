using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace Verification.settings {
    public class RateSettingsController {
        private RateSettingsForm view = null;
        FileSettingHandler<Settings> fileHandler = new FileSettingHandler<Settings>();
        // значения при открытии
        private int meassureIndex = 1;
        private double min = 40;
        private double max = 20;

        public double Min { get => meassureIndex==0? min: max * min / 100; }
        public double Max { get => max; }

        /// <summary>
        /// Создание или фокус формы 
        /// </summary>
        public void createView() {
            // create or focus help form 
            if (view != null && !view.Disposing && view.Text != "") {
                view.Focus();
            } else {
                view = new RateSettingsForm(this);
                view.Show();  
            }
            fillFields();
        }
        
        /// <summary>
        /// Заполнить поля формы из текущих значений
        /// </summary>
        private void fillFields() {
            view.fillFormByDefault(min.ToString(), max.ToString(), meassureIndex);

        }
        /// <summary>
        /// Сохранение текущих значений и выход из формы
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        internal void onOk(string min, string max) {
            if (!validateForm(min, max))
                return;
            this.min = Double.Parse(min);
            this.max = Double.Parse(max);
            view.Dispose();
        }

        /// <summary>
        /// Отмена
        /// </summary>
        internal void onCancel() {
            view.Dispose();
        }

        /// <summary>
        /// При изменении выбранной единицы счисления для минимальной оценки
        /// </summary>
        /// <param name="index"></param>
        internal void onCbChanged(int index) {
            meassureIndex = index;
        }


        /// <summary>
        /// Проверка, что все поля формы заполнены
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private string checkFilled(string min, string max) {
            if (max == "") return "Заполните поле Max";
            if (min == "") return "Заполните поле Min";
            return "";
        }
        /// <summary>
        /// Проверка, что минимальный балл меньше максимального или меньше 100%
        /// </summary>
        /// <param name="minStr"></param>
        /// <param name="maxStr"></param>
        /// <returns></returns>
        private string checkMinBoarders(string minStr, string maxStr) {
            double min=0, max=0;
            try {
                min = Double.Parse(minStr);
                if (min < 0) throw new Exception();
            }catch(Exception) {
                return "Поле MIN не является положительным действительным числом";
            }
            try {
                max = Double.Parse(maxStr);
                if (max < 0) throw new Exception();
            } catch (Exception) {
                return "Поле MAX не является положительным действительным числом";
            }

            if (meassureIndex == 0) {
                return min <= max ? "" : "Минимальный балл должен быть меньше максимального";
            } else {
                return min <= 100 ? "" : "Минимальный балл должен быть меньше 100%";
            }
        }

        /// <summary>
        /// Валидация формы
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private bool validateForm(string min, string max) {
            var msg = checkFilled(min, max);
            if (msg != "") {
                view.ShowMsg(msg, "");
                return false;
            }
            msg = checkMinBoarders(min, max);
            if (msg != "") {
                view.ShowMsg(msg, "");
                return false;
            }
            return true;
        }

        private void openOpenFileDialog() {
            var openFileDialog = new OpenFileDialog {
                Title = "Выберите json файл",
                Multiselect = false,
                Filter = "Файл json|*.json"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                var fileName = openFileDialog.FileName;
                deserializeSettings(fileName);
                view.fillFormByDefault(min.ToString(), max.ToString(), meassureIndex);
            }
        }

        
        private void deserializeSettings(string fileName) {
            Func<object, Boolean> checker = (o) => {
                var settings = (Settings)o;
                try {
                    if (settings.Index > 1 || settings.Index < 0)
                        throw new InvalidCastException();
                } catch (Exception e) { throw e; }
                return true;
            };
            try {
                var sett = fileHandler.readFromFile(fileName, checker);
                this.meassureIndex = sett.Index;
                max = sett.Max;
                min = sett.Min;
            } catch (Exception e) {
                view.ShowMsg(e.Message, "Exception!");
            }
            
        }
        private void openSaveFileDialog() {
            var saveDialog = new SaveFileDialog {
                Title = "Сохранение списка ошибок",
                FileName = "Settings.json",
                Filter = "Текстовый документ (*.json)|*.json"
            };
            if (saveDialog.ShowDialog() == DialogResult.OK) {
                var settings = new Settings();
                settings.Max = max;
                settings.Min = min;
                settings.Index = meassureIndex;
                fileHandler.writeInFile(saveDialog.FileName, settings);
            }
        }

        internal void import() {
            openOpenFileDialog();
        }

        internal void export(string min, string max) {
            if (validateForm(min, max)) {
                this.max = Double.Parse(max);
                this.min = Double.Parse(min);
                openSaveFileDialog(); 
            }
        }

        /// <summary>
        /// Класс для сериализации настроек
        /// </summary>
        [Serializable]
        private class Settings {
            public double Min { get; set; }
            public double Max { get; set; }
            public int Index { get; set; }
        }
    }
}
