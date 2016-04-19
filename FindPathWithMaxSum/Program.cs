using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FindPathWithMaxSum
{
    class Report
    {
        public List<Node> PathWithMaxSum = null;
        public int MaxSum;
        public int PathCount;
    }

    class Node
    {
        public int Value;
        public int Row;
        public int Column;
        public int MaxElements;
        public Node(int value, int row, int column, int maxElements) 
        { 
            Value = value;
            Row = row;
            Column = column;
            MaxElements = maxElements;
        }

        public List<Node> Nodes = new List<Node>();
        public override string ToString()
        {
            return Value.ToString(); 
        }

        public void Explore(Report report, List<Node> path, Node node)
        {
            List<Node> copyPath = new List<Node>(path);

            if (!copyPath.Exists(x => x == node))
            {
                copyPath.Add(node);
                report.PathCount++;
                if (copyPath.Count >= MaxElements)
                {                     
                    var sum = copyPath.Sum(d => d.Value);
                    if (report.MaxSum < sum)
                    {
                        report.MaxSum = sum;
                        report.PathWithMaxSum = copyPath;                        
                        Console.WriteLine("max sum = " + report.MaxSum);
                        OutputPath(copyPath);      
                    }

                    if (report.PathCount % 100000 == 0) 
                    { 
                        Console.Write("                      \r" + report.PathCount); 
                    }
                    return;
                }

                // Iterate each node recursively.
                foreach (var item in node.Nodes)
                {
                    item.Explore(report, copyPath, item);                    
                }
            }
        }
        
        public void OutputPath(List<Node> path)
        {
            var output = new StringBuilder();
            foreach (var item in path)
            {
                output.Append("->" + item.ToString());
            }
            Console.WriteLine(output.ToString());
        }
    }

    class Program
    {
        int rowCount;
        int colCount;
        int[,] valueArray;
        int[] valueMap;
        int maxElements;
        List<Node> tree = new List<Node>();
        List<List<Node>> Paths = new List<List<Node>>();
        Report report = new Report();

        public void Initialize(int[,] array)
        {
            this.valueArray = array;
            rowCount = array.GetLength(0);
            colCount = array.GetLength(1);
            valueMap = new int[rowCount * colCount];
            int seq = 0;
            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    this.valueMap[seq++] = valueArray[row, col];
                    tree.Add(new Node(valueArray[row, col], row, col, maxElements));                    
                }
            }

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    AddConnections(tree[row * colCount + col], row, col);
                }
            }
        }

        private void AddConnections(Node node, int row, int column)
        {
            // look up
            if (row > 0)
            {
                node.Nodes.Add(tree[(row - 1) * colCount + column]);
            }
            // look right
            if (column < colCount - 1)
            {
                node.Nodes.Add(tree[row * colCount + (column + 1)]);
            }
            // look down
            if (row < rowCount - 1)
            {
                node.Nodes.Add(tree[(row + 1) * colCount + column]);
            }
            // look left
            if (column > 0)
            {
                node.Nodes.Add(tree[row * colCount + (column - 1)]);
            }
        }

        static void Main(string[] args)
        {
            int[,] grid = {{45,16,46,14,47,13,48,12,49,11,50,10},       
                           {16,46,13,43,16,47,12,43,17,42,13,48},       
                           {49,16,42,19,40,12,47,19,40,18,42,13},       
                           {17,42,19,40,20,41,23,39,18,46,11,48},       
                           {40,19,43,18,42,19,46,20,48,21,43,19},       
                           {18,50,12,48,17,42,18,42,17,46,19,51},       
                           {51,12,51,10,53,10,42,11,42,12,43,18},       
                           {18,42,18,42,19,49,18,40,21,39,17,42},       
                           {41,17,47,13,49,10,49,23,52,13,43,10},       
                           {10,45,13,42,17,46,19,48,12,47,1,54},       
                           {53,10,43,12,41,13,43,11,44,10,46,19},       
                           {10,37,19,47,19,52,18,53,11,53,10,41}};
            //int[,] grid = {{45,16,46,14},       
            //               {16,46,13,43},       
            //               {49,16,42,19},       
            //               {17,42,19,40}};
            //int[,] grid = {{45,16,46},       
            //               {16,46,13},       
            //               {49,16,42}};
            
            Program p = new Program();
            p.maxElements = 40;
            p.Initialize(grid);

            Console.WriteLine("Maximum Sum = " + p.FindPathWithMaxSum());
            Console.Write("Path with Max Sum = "); p.report.PathWithMaxSum[0].OutputPath(p.report.PathWithMaxSum);
            Console.WriteLine("Paths Iterated = " + p.report.PathCount);

        }

        private int FindPathWithMaxSum()
        {       
            var path = new List<Node>();
            for (int i = 0; i < tree.Count; i++)
            {
                Console.WriteLine("Array index: " + i);
                if (!Paths.Exists(x => x[0] == tree[i]) &&
                    !Paths.Exists(x => x[x.Count - 1] == tree[i]))
                {
                    tree[i].Explore(report, path, tree[i]);
                }
            }
            return report.MaxSum;
        }
        
    }
}
