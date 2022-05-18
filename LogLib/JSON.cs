using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.IO;

namespace LogLib
{
	class JSON
	{
        public static int Serializer(Log log, string logPath, out string exception)
        {
            exception = "";
            try
            {
                Trace.WriteLine("Начинаем сериализацию");
                var options = new JsonSerializerOptions
                {
                    //Включвает кирилицу
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                    //Включает перенос строки поле ккаждого поля
                    //WriteIndented = true
                };
                /*
                 * Возникает исключение IOException: The process cannot access the file 'file path' because it is being used by another process C#
                 * В параметрах исключений => Common Language надо поднять флак System.IO.IOException
                 */
                using (FileStream fileStream = new FileStream(logPath, FileMode.OpenOrCreate))
                {
                    fileStream.Seek(0, SeekOrigin.End); // Перевод курсора в конец файла
                    fileStream.Write(Encoding.Default.GetBytes("\n")); // Добавление новой строуи
                    string json = JsonSerializer.Serialize(log, options); //Составляем строку
                    Trace.WriteLine(json);
                    fileStream.Write(Encoding.Default.GetBytes(json)); // записываем строку
                    fileStream.Close(); // Закрываем эту коробку пандоры
                }
                return 0;
            }
            catch (Exception ex)
            {
                exception = String.Concat(ex.ToString(), " ", ex.Message);
                return 1;
            }
        }
    }
}
