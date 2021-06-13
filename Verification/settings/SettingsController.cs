using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Verification.settings {
    class SettingsController<T> {
        //    private IView view = null;
        //    FileSettingHandler<T> fileHandler = new FileSettingHandler<T>();
        //    ISettingsController specificContr;
        //    IModel model;

        //    public SettingsController(IView view, ISettingsController specificContr, IModel model) {
        //        this.view = view ?? throw new ArgumentNullException(nameof(view));
        //        this.specificContr = specificContr ?? throw new ArgumentNullException(nameof(specificContr));
        //        this.model = model ?? throw new ArgumentNullException(nameof(model));
        //    }

        //    //o
        //    public void createView() {
        //        // create or focus help form 
        //        if (view != null && !view.Disposing && view.Text != "") {
        //            view.Focus();
        //        } else {
        //            view = ;
        //            view.setController(this);
        //            view.Show();
        //        }
        //        fillForm();
        //    }
        //    //o
        //    private void fillForm() {
        //        //view.fillFields();
        //    }
        //    //i
        //    bool validateForm() { return true; }
        //    //i
        //    void saveState() { }

        //    //o?
        //    internal void onOk() {
        //        if (!validateForm())
        //            return;
        //        saveState();
        //        view.Dispose();
        //    }

        //    //o
        //    internal void onCancel() {
        //        view.Dispose();
        //    }
        //    //o
        //    private void openOpenFileDialog() {
        //        var openFileDialog = new OpenFileDialog {
        //            Title = "Выберите json файл",
        //            Multiselect = false,
        //            Filter = "Файл json|*.json"
        //        };

        //        if (openFileDialog.ShowDialog() == DialogResult.OK) {
        //            var fileName = openFileDialog.FileName;
        //            deserializeSettings(fileName);
        //            //view.fillFields();
        //        }
        //    }

        //    //o?
        //    private void deserializeSettings(string fileName) {
        //        Func<object, Boolean> checker = (o) => {
        //            return true;
        //        };
        //        try {
        //            var sett = fileHandler.readFromFile(fileName, checker);
        //            // сохранить настройки в поля
        //        } catch (Exception e) {
        //            //view.ShowMsg(e.Message, "Exception!");
        //        }

        //    }
        //    //o?
        //    private void openSaveFileDialog() {
        //        var saveDialog = new SaveFileDialog {
        //            Title = "Сохранение списка ошибок",
        //            FileName = "Settings.json",
        //            Filter = "Текстовый документ (*.json)|*.json"
        //        };
        //        if (saveDialog.ShowDialog() == DialogResult.OK) {
        //            //var settings = createSettingsObject();
        //            //fileHandler.writeInFile(saveDialog.FileName, settings);
        //        }
        //    }
        //    //o
        //    internal void import() {
        //        openOpenFileDialog();
        //    }

        //    //o?
        //    internal void export(string min, string max) {
        //        if (validateForm()) {
        //            saveState();
        //            openSaveFileDialog();
        //        }
        //    }
        //    //T createSettingsObject() {
        //    //    return new T();
        //    //}

        //    private class Settings {
        //        public IDictionary<string, string> settings = new Dictionary<string, string>();
        //    }
    }
}
