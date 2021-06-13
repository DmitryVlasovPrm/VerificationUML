using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Verification.settings {
    /// <summary>
    /// Класс для дессериал\сериал json
    /// </summary>
    class FileSettingHandler<T> {
        public T readFromFile(string fileName, Func<object, Boolean> checker) {
            var jsonString = File.ReadAllText(fileName);
            T output;
            try {
                output = JsonSerializer.Deserialize<T>(jsonString);
                checker(output);
            } catch (InvalidCastException) {
                throw new Exception("Проблема с индексом для комбобокса");
            } catch (Exception e) when (e is ArgumentNullException || e is JsonException || e is NotSupportedException) {
                throw new Exception("Проблема с чтением файла");
            } catch (Exception e) when (e is InvalidCastException || e is FormatException) {
                throw new Exception("Значения настроек не соответствуют требуемым. Проверьте, что используются только действительные и целые числа");
            }
            return output;
        }
        public void writeInFile(string fileName, T input) {
            var jsonString = JsonSerializer.Serialize(input);
            File.WriteAllText(fileName, jsonString);
        }
    }
}
