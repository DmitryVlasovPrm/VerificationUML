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
        Settings curSettings = new Settings(40, 20, 1);

        public double Min { get => curSettings.Index == 0 ? curSettings.Min : curSettings.Max * curSettings.Min / 100; }
        public double Max { get => curSettings.Max; }

        /// <summary>
        /// Создание или фокус формы 
        /// </summary>
        public void createView() {
            // create or focus help form 
            if (view != null && !view.Disposing && view.Text != "") {
                view.Focus();
            } else {
                view = new RateSettingsForm();
                view.setController(this);
                view.Show();
            }
            fillForm();
        }

        /// <summary>
        /// Заполнить поля формы из текущих значений
        /// </summary>
        private void fillForm() {
            view.fillFields(curSettings.Min.ToString(), curSettings.Max.ToString(), curSettings.Index);
        }
        /// <summary>
        /// Сохранение текущих значений и выход из формы
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        internal void onOk(string min, string max) {
            if(validateAndSaveState(min, max))
                view.Dispose();
        }
        private bool validateAndSaveState(string min, string max) {
            var msg = Validator.validateForm(min, max, curSettings.Index);
            if (msg != "") {
                view.ShowMsg(msg, "");
                return false;
            }
            curSettings.Min = Double.Parse(min);
            curSettings.Max = Double.Parse(max);
            return true;
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
            curSettings.Index = index;
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
                view.fillFields(curSettings.Min.ToString(), curSettings.Max.ToString(), curSettings.Index);
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
                curSettings = fileHandler.readFromFile(fileName, checker);
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
                fileHandler.writeInFile(saveDialog.FileName, curSettings);
            }
        }

        internal void import() {
            openOpenFileDialog();
        }

        internal void export(string min, string max) {
            if (validateAndSaveState(min, max))
                openSaveFileDialog();
        }

        /// <summary>
        /// Класс для сериализации настроек
        /// </summary>
        [Serializable]
        private class Settings {
            public Settings(double min, double max, int index) {
                Min = min;
                Max = max;
                Index = index;
            }

            public double Min { get; set; }
            public double Max { get; set; }
            public int Index { get; set; }
        }
        private class Validator {
            /// <summary>
            /// Проверка, что все поля формы заполнены
            /// </summary>
            /// <param name="min"></param>
            /// <param name="max"></param>
            /// <returns></returns>
            private static string checkFilled(string min, string max) {
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
            private static string checkMinBoarders(string minStr, string maxStr, int index) {
                double min = 0, max = 0;
                try {
                    min = Double.Parse(minStr);
                    if (min < 0) throw new Exception();
                } catch (Exception) {
                    return "Поле MIN не является положительным действительным числом";
                }
                try {
                    max = Double.Parse(maxStr);
                    if (max < 0) throw new Exception();
                } catch (Exception) {
                    return "Поле MAX не является положительным действительным числом";
                }

                if (index == 0) {
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
            public static string validateForm(string min, string max, int index) {
                var msg = checkFilled(min, max);
                if (msg != "") return msg;
                msg = checkMinBoarders(min, max, index);
                return msg;
            }
        }
    }
}
