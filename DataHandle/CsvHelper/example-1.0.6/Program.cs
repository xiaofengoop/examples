using CalculateClass;
using CsvHelper;

// basic usage
{
    // 1、create a csv file
    {
        // create a csv object to create csv file.
        var csvc = new Csv();

        // add column names to this object.
        // Adding is only possible when there are no column names; otherwise, it returns false.
        _ = csvc.SetColumnNames(new string[3] { "id", "sex", "age" });

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

            // add data.
            // If there are no column names or
            // if the column count doesn't match the incoming data, it will return false.
            foreach (var data in datas)
            {
                _ = csvc.AddRow(data);
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
            _ = csvc.AddColumn(columnName, columnData);
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
        // It will return false if the column name doesn't exist.
        _ = csvr.ModifyColumnName("language", "message");

        // delete the column named "sex" and return the deleted values.
        // It will return false if the column name doesn't exist.
        _ = csvr.DeleteColumn("sex", out string[]? values);

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

    // 3、handle data
    {
        // read csv file.
        var csvRead = new Csv("2.csv", @"\N");

        // copy csv file to handle data.
        var csvHandle = csvRead.Copy();

        // delete data with types other than "single".
        csvHandle.DeleteColumn("message", out _);
        csvHandle.DeleteColumn("id", out _);

        // show data.
        csvRead.Show();

        // newline
        Console.WriteLine();

        // display relevant information
        csvHandle.DisplayRelevantInformation();

        var max = csvRead.Max("age");
        var min = csvRead.Min("age");
        var average = csvRead.Average("age");
        var variance = csvRead.Variance("age");

        // Choose the smaller or the larger one.
        var median = csvRead.Median("age", BasicCal.Media.Lower);
        var percent10 = csvRead.Percent("age", BasicCal.Media.Greater, (1.0f / 10));
    }
}