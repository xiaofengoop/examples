using CalculateClass;
using CsvHelper;
using DataHandleExtension;
using DataHandleInterface;
using IniHelper;

// CsvHelper
{
    // create a csv file
    {
        // create a csv object to create csv file.
        var csvc = new Csv();

        // add column names to this object.
        // Adding is only possible when there are no column names.
        csvc.SetColumnNames("id", "sex", "age");
        // or csvc.SetColumnNames(new[] { "id", "sex", "age" });

        // add datas, default order is column name order.
        {
            // create datas.
            var datas = new string[3][]
            {
                new string[3] { "one", "1", "28" },
                new string[3] { "two", "0", "18" },
                new string[3] { "three", "1", "89" }
                        };

            // add data.
            foreach (var data in datas)
            {
                csvc.AddRow(data);
            }

            // or
            csvc.AddRow("four", "0", "27");
        }

        // save the csv file
        csvc.Save("1.csv");

        // add a column to the object.
        {
            // create a column data.
            var columnName = "message";
            var columnData = new string[4]
            {
                "I'm so handsome!",
                "You look so beautiful",
                "I'm going to die",
                "I feel very tired every day"
                        };

            // add the column
            csvc.AddColumn(columnName, columnData);
        }

        // save the csv file and watch what happens
        csvc.Save("2.csv");

        // display the string for repesenting null values in the file
        Console.WriteLine(csvc.Null);
    }

    // newline
    Console.WriteLine();

    // modify a csv file
    {
        // create a csv object for reading csv files and set its null values to "\N"
        var csvr = new Csv("example.csv", @"\N");

        // get all columns, returning a dic<str: columnName, str: data>[] type data
        var fileRows = csvr.Rows;

        // get all columns, returning a dic<str: columnName, str[]: data> type data
        var fileColumns = csvr.Columns;

        // show these data
        {
            // show rows
            int i = 0;
            foreach (var row in fileRows)
            {
                Console.WriteLine(i++);
                foreach (var data in row)
                {
                    Console.WriteLine($"Key: {data.Key}, Value: {data.Value}");
                }
            }

            Console.WriteLine();

            // show columns
            foreach (var column in fileColumns)
            {
                Console.WriteLine($"Key: {column.Key}\r\nValues:");
                foreach (var data in column.Value)
                {
                    Console.WriteLine(data);
                }
            }

            // show colum names
            // get columnNames
            var names = csvr.ColumnNames;
            Console.WriteLine("colum names:");
            foreach (var name in names)
            {
                Console.WriteLine(name);
            }

            // show csv data
            Console.WriteLine("csv data:");
            csvr.Show();
        }

        // modify a column name.
        csvr.ModifyColumnName("language", "message");

        // delete the column named "sex" and return the deleted values.
        csvr.DeleteColumn("sex", out string[]? values);

        // delete rows
        csvr.DeleteRow(17, out _);
        csvr.DeleteRow(columns => columns["is_city"] == "1");

        // show deleted values
        foreach (var value in values!)
        {
            Console.WriteLine(value);
        }

        // modify a data
        csvr[1, "message"] = "中文测试";

        // get a data
        _ = csvr[0, "message"];

        // get a row
        _ = csvr[2];

        // get a column
        _ = csvr["id"];

        // get data count
        Console.WriteLine($"csv row count: {csvr.Length}");

        // get column count
        Console.WriteLine($"csv column count: {csvr.ColumnCount}");

        //get all data count
        Console.WriteLine($"csv data count: {csvr.Count}");

        // save this csv file containing Chinese characters
        csvr.Save("exampleModify.csv", Csv.Language.Chinese);
    }

    // newline
    Console.WriteLine();

    // copy data
    {
        var csvr = new Csv("2.csv", @"\N");

        // Copy CSV object - Modifying values on the current CSV object will affect the original CSV object (columns and rows will not be affected).
        var csvrCopy = csvr.Copy();

        // Clone CSV object - Modifying values on the current CSV object will not affect the original CSV object.
        var csvrClone = csvr.Clone();


        // test
        csvrCopy.DeleteColumn("sex", out _);

        csvrCopy[0, "message"] = "我被Copy修改了";

        csvrClone.DeleteColumn("age", out _);

        csvrClone[2, "message"] = "我被Clone修改了";

        Console.WriteLine("csvr show");
        csvr.Show();

        Console.WriteLine("csvrCopy show");
        csvrCopy.Show();

        Console.WriteLine("csvrClone show");
        csvrClone.Show();
    }

    // exception
    {
        // csv class
        try
        {

        }
        // All csv exceptions inherit from CsvException (except not find index and so then) 
        catch (CsvException error)
        {
            var exceptionName = error.GetType().Name;
            Console.WriteLine(exceptionName + ':' + error.Message);
            if (error.InnerException != null)
                throw error.InnerException;
        }
    }
}

// CalculateClass
{
    // Handle data
    {
        // read csv file.
        var csvRead = new Csv("2.csv", @"\N");

        var csvCopy = csvRead.Copy();

        // delete data with types other than "single".
        csvCopy.DeleteColumn("message", out _);
        csvCopy.DeleteColumn("id", out _);

        // show data.
        csvCopy.Show();

        // newline
        Console.WriteLine();

        // display relevant information
        csvCopy.DisplayRelevantInformation();
        // or you can csvCopy.DisplayRelevantInformation(string path) to save info message.

        var max = csvCopy.Max("age");
        var min = csvCopy.Min("age");
        var average = csvCopy.Average("age");
        var variance = csvCopy.Variance("age");

        // Choose the smaller or the larger one.
        var median = csvCopy.Median("age", BasicCal.Skewness.Lower);
        var percent10 = csvCopy.Percent("age", BasicCal.Skewness.Greater, (1.0f / 10));
    }
}

Console.WriteLine();

// CsvExtension
{
    // csv to idataview -- need install ml nuget and CodeAnalysis
    var csv = new Csv();
    csv.SetColumnNames("name", "age", "gender");
    csv.AddRow("xiaoF", "20", "true");
    csv.AddRow("test", "20", "false");
    var data = csv.GetIDataView(new Dictionary<string, Type> { { "age", typeof(int) }, { "gender", typeof(bool) } });


    // or

    // Generate a .cs file for the corresponding class based on the CSV object.
    var csvDataView = new Csv();
    csvDataView.SetColumnNames("name", "age", "sex");
    csvDataView.CreateObjectClass("Person", new Dictionary<string, Type>
    { { "age", typeof(int) }, { "sex", typeof(bool) } });

    // Convert CSV class data to ML.IDataView.
    {
        csvDataView.AddRow("xiaoF", "20", "true");
        csvDataView.AddRow("test", "20", "false");

        // Since the return value is DataView, you need to install ml nuget.
        var dataView = csvDataView.GetIDataView<Person>();
    }
    // or you can get objects
    {
        var dataOther = csvDataView.GetObjects<Person>();
    }
}

Console.WriteLine();

// csvhelper 2.4.3 update
{
    var csv = new Csv();
    csv.SetColumnNames("test");

    csv.AddRow(new Dictionary<string, string> { { "test", "1" } });

    _ = csv.IsBooleanOfColumn("test");
    _ = csv.IsIntOfColumn("test");
    _ = csv.IsSingleOfColumn("test");

    var selectdataOfCsv = csv[data => data["test"] == "1", csv.IsIntOfColumn];

}

// IniHelper
{
    // create a ini file
    {
        var ini = new Ini();
        ini.AddOption(new Ini.Option("test") { Annotation = "test" });
        ini.AddOption(new Ini.Option("try", "356"));
        var section = new Ini.Section("No.1");
        section.AddOption(new Ini.Option("test", "345") { Annotation = "test" });
        section.AddOption(new Ini.Option("mm"));
        section.DeleteOption("mm");
        section.Annotation = "test";
        ini.AddSection(section);
        ini.Save("3.ini");
    }

    // load ini file
    {
        var ini = new Ini("3.ini");
        if (ini.ContainSection("test"))
            ini.DeleteSection("test");

        if (ini.ContainOption("test"))
            ini.DeleteOption("test");

        if (ini.ContainSection("No.1"))
        {
            var section = ini.GetSection("No.1");
            if (section.ContainOption("test"))
            {
                var option = section.GetOption("test");
                var name = option.Key;
                var value = option.Value;
                var ann = option.Annotation;
            }
        }

        ini.Save("4.ini");
    }

    var iniShow = new Ini("3.ini");
    iniShow.Show();
}
Console.WriteLine("finally");

/// <summary>
/// Person Class
/// </summary>
class Person
{
    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    [CsvExtension.CorrespondingCsvColumnName("sex")]
    public bool Gender { get; set; }

}