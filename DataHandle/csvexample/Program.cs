using CsvHelper;

// basic usage
{
    // 1、create a csv file
    {
        // create a csv object to create csv file.
        var csvc = new Csv();

        // add column names to this object. the default data type is string.
        csvc.SetColumnNames(new string[3] { "id", "sex", "age" });

        // add datas, default order is column name order.
        {
            // create datas.
            var datas = new string[4][]
            {
                new string[3] { "one", "1", "28" },
                new string[3] { "two", "0", "18" },
                new string[3] { "three", "1", "89" },
                new string[3] { "four", "0", "27" }
            };

            // add datas.
            foreach (var data in datas)
            {
                csvc.AddRow(data);
            }
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

    // 2、modify a csv file
    {
        // create a csv object for reading csv files and set its null values to "\N"
        var csvr = new Csv("example.csv", @"\N");

        // get all columns, returning a dic<str: columnName, str: data>[] type data
        var fileRows = csvr.Rows;

        // get all columns, returning a dic<str: columnName, str[]: data> type data
        var fileColumns = csvr.Columns;

        // show these datas
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
        }

        // modify a column name.
        csvr.ModifyColumnName("language", "message");

        // delete the column named "sex" and return the deleted values.
        csvr.DeleteColumn("sex", out string[]? values);

        // show values
        foreach (var value in values!)
        {
            Console.WriteLine(value);
        }

        // modify a data
        csvr["message", 1] = "中文测试";

        // get a data
        _ = csvr["message", 0];

        // get a row
        _= csvr[2];

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
}