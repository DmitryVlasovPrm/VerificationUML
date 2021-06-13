using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Verification.settings {
    public class MistakesSettingsController {
        private MistakesSettingsForm view = null;
        FileSettingHandler<Settings> fileHandler = new FileSettingHandler<Settings>();
        public void createView() {
            // create or focus help form 
            if (view != null && !view.Disposing && view.Text != "") {
                view.Focus();
            } else {
                view = new MistakesSettingsForm(this);
                view.Show();
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
                // заполнить настройки ...
                fileHandler.writeInFile(saveDialog.FileName, settings);
            }
        }
        private void openOpenFileDialog() {
            var openFileDialog = new OpenFileDialog {
                Title = "Выберите json файл",
                Multiselect = false,
                Filter = "Файл json|*.json"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                var fileName = openFileDialog.FileName;
                // дессериализация ...
                // заполнить форму ...
            }
        }

        private class Settings {
            Mistake[] mistakes;
        }
    }
}
