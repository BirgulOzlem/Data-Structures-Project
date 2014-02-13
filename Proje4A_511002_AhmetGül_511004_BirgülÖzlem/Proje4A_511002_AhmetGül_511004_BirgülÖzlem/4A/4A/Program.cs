using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace dijk_1
{
    class PriorityQ
    {
        // array in sorted order, from max at 0 to min at size-1
        private readonly int SIZE = 20;
        private Edge[] queArray;
        private int size;

        public PriorityQ() // constructor
        {
            queArray = new Edge[SIZE];
            size = 0;
        }

        public void insert(Edge item) // insert item in sorted order
        {
            int j;
            for (j = 0; j < size; j++) // find place to insert
                if (item.distance >= queArray[j].distance)
                    break;
            for (int k = size - 1; k >= j; k--) // move items up
                queArray[k + 1] = queArray[k];
            queArray[j] = item; // insert item
            size++;
        }

        public Edge removeMin() // remove minimum item
        {
            return queArray[--size];
        }
        public void removeN(int n) // remove item at n
        {
            for (int j = n; j < size - 1; j++) // move items down
                queArray[j] = queArray[j + 1];
            size--;
        }
        public Edge peekMin() // peek at minimum item
        {
            return queArray[size - 1];
        }
        public int getSize() // return number of items
        {
            return size;
        }
        public bool isEmpty() // true if queue is empty
        {
            return (size == 0);
        }
        public Edge peekN(int n) // peek at item n
        {
            return queArray[n];
        }
        public int find(int findDex) // find item with specified
        { // endVertex value
            for (int j = 0; j < size; j++)
                if (queArray[j].endVertex == findDex)
                    return j;
            return -1;
        }
    } // end class PriorityQ

    class Edge
    {
        public int startVertex; // index of a vertex starting edge
        public int endVertex; // index of a vertex ending edge
        public int distance; // distance from src to dest

        public Edge(int sv, int dv, int d) // constructor
        {
            startVertex = sv;
            endVertex = dv;
            distance = d;
        }

    } // end class Edge

    class Edge_other
    {
        public string vertex1;
        public string vertex2;
        public int distance;

        public Edge_other(string vertex1, string vertex2,int distance)
        {
            this.vertex1 = vertex1;
            this.vertex2 = vertex2;
            this.distance = distance;
        }
    }

    class Vertex
    {
        public string harf_adı; // label (e.g. ‘A’)
        public bool isInTree;
        public Vertex(string lab) // constructor
        {
            harf_adı = lab;
            isInTree = false;
        }
        public void display()
        {
            Console.WriteLine(" " + harf_adı);
        }
        // -------------------------------------------------------------
    } // end class Vertex

    class Dijkstra
    {
        public int[,] distanceMat;      // her köşenin diğer köşeler ile arasındaki uzaklık veya ağırlık matrisi
        public int[] D;         // 0. köşeden diğer köşelere ulan en kısa yolları tutan matris
        public int[] Nodes;         // -1 olan ilgili indis o köşeden geçildiği gösteriyor
        public int tur1 = 0;
        public int tur2 = 0;
        public int ilk;     // kullanıcıdan alınan "nereden" düğümü
        public int son;     // kullanıcıdan alınan "nereye" düğümü
        public bool[] kontrol = new bool[10];
        
        public Dijkstra(int boyut, int[,] pArray, int a, int b)
        {
            distanceMat = new int[boyut, boyut];
            Nodes = new int[boyut];
            D = new int[boyut];
            tur1 = boyut;
            ilk = a;
            son = b;
            
            for (int i = 0; i < tur1; i++)
            {
                for (int j = 0; j < tur1; j++)
                {
                    distanceMat[i, j] = pArray[i, j];   // iki for ile main de random sayılar ile oluşturulup yollanmış olan matrisi
                }                                               // distanceMat matrisine kopyaladık.
            }
            for (int i = 0; i < tur1; i++)
            {
                Nodes[i] = i;           // dolaşılan her düğümü -1 yapmadan önce hepsine -1 den farklı olarak indeksini atadık
            }
            Nodes[ilk - 1] = -1;    // kullanıcıdan alınan "nereden" düğümü ile başladık ve -1 atadık
            for (int i = 0; i < tur1; i++)
            {
                D[i] = distanceMat[ilk - 1, i];     // D[] matrisine "nereden" düğümünün diğer tüm düğümler ile olan ilk uzaklıklarını atadık
            }
        }

        public void DijkstraSolving(Queue que, int[] min_bulan_dizi, int current_tour)
        {
            int a = 0;
            int minValue = Int32.MaxValue;  // sonsuz olmasa da en yüksek integer değerden başlayarak bulabildiğimiz en kısa yolu buluyoruz
            int minNode = 0;
            for (int i = 0; i < tur1; i++)
            {
                if (Nodes[i] == -1)     // eğer ilgili düğümün Nodes[] değeri -1 ise daha önce o düğümden geçilmiş demektir
                    continue;               // o sebeple continue diyerek for döngüsüne devam ediyoruz
                if (D[i] > 0 && D[i] < minValue)
                {                       // eğer o düğümden ilk kez geçilecekse ve D[] değeri şimdiye kadarki ağırlığından küçük ise
                    minValue = D[i];            // yeni ve daha kısa bir yol bulmuşuz demektir
                    minNode = i;                        // o düğümün ağırlığını değiştiriyoruz
                }
            }

            Nodes[minNode] = -1; // for döngüsünden çıktıktan Nodes[] daki ilgili düğümü -1 yaparak, o düğümden geçtiğimizi belirtiyoruz

            int [] en_kucuk = new int[tur1];

            for (int i = 0; i < tur1; i++)
            {
                if ((D[minNode] + distanceMat[minNode, i]) < D[i] && distanceMat[minNode, i] != 0)
                {  // i'nin bu yeni düğümden önceki en kısa yolunun ve distanceMat[minNode, i] deki uzunluğunun toplamı
                    D[i] = minValue + distanceMat[minNode, i];// D[i] den küçükse daha kısa yol bulunmuş demektir, atıyoruz.
                      en_kucuk[i] = minNode+1;
                }
            }

            que.Enqueue(en_kucuk[son-1]);
        } 

        public void run(Queue que, int[] min_bulan_dizi)
        {
            for (tur2 = 1; tur2 < tur1; tur2++)     // boyut kadar dönüyoruz
            {
                DijkstraSolving(que, min_bulan_dizi,tur2);
                Console.WriteLine("\n\nYineleme: " + tur2);     // her for döngüsünde Nodes[] ile şuanki düğümü ayrıca kullanıcıdan alınan
                Console.Write("D[] <en kisa yol>           ");      // "nereden" düğümünden tüm düğümlere giden en kısa yolları D[] ile takip edebiliyoruz.
                for (int i = 0; i < tur1; i++)
                    Console.Write(D[i] + "  ");
                Console.WriteLine("");
                Console.Write("Nodes[] <geçilen dügümler> ");
                for (int i = 0; i < tur1; i++)
                    Console.Write(Nodes[i] + "  ");
                Console.WriteLine("");
                Console.WriteLine("\n *** " + D[son - 1]);
            }
        }

        public void bfs(Vertex[] vertex_list, int start)    // breadth-first search --> derinlik öncelikli dolaşma
        {
            Queue kuyruk = new Queue();
            int v2;
            kontrol[start] = true;      // geçtiğimiz düğümü işaretliyoruz
            vertex_list[start].display();       // ilk olarak başladığımız düğümü ekrana görüntületiyoruz
            kuyruk.Enqueue(start);      // başlamak istediğimiz düğümü kuyruğumuza ekliyoruz
            
            while (kuyruk.Count != 0)       // kuyruk boşalıncaya kadar döngüden çıkmıyoruz
            {
                int v1 = (int)kuyruk.Dequeue();     // kuyruktan sıradaki düğümü çıkarıyoruz
                
                while ((v2 = getAdjUnvisitedVertex(v1)) != -1)     // işaretlenmemiş komşumuz kalana kadar dönüyoruz
                { 
                    kontrol[v2] = true;     // geçtiğimiz düğümü işaretliyoruz
                    vertex_list[v2].display();      //işaretlediğimiz düğümü ekrana görüntületiyoruz
                    kuyruk.Enqueue(v2);         // ve kuyruğa ekliyoruz.
                } 
            }

            for (int j = 0; j < kontrol.Count(); j++)    // kontrol dizimizi default haline geri döndürüyoruz
                kontrol[j] = false;
        }

        public int getAdjUnvisitedVertex(int v)
        {
            for (int j = 0; j < tur1; j++)
                if (distanceMat[v, j] >= 1 && kontrol[j] == false)
                    return j;
            return -1;
        } // end getAdjUnvisitedVertex()

    }

    class Graph
    {
        private readonly int MAX_VERTS = 20;
        private readonly int INFINITY = 1000000;
        private Vertex[] vertexList; // list of vertices
        private int[,] adjMat; // adjacency matrix
        private int nVerts; // current number of vertices
        private int currentVert;
        private PriorityQ thePQ;
        private int nTree; // number of verts in tree

        public Graph() // constructor
        {
            vertexList = new Vertex[MAX_VERTS];
            adjMat = new int[MAX_VERTS, MAX_VERTS]; // adjacency matrix
            nVerts = 0;
            for (int j = 0; j < MAX_VERTS; j++) // set adjacency
                for (int k = 0; k < MAX_VERTS; k++) // matrix to 0
                    adjMat[j, k] = INFINITY;
            thePQ = new PriorityQ();
        } // end constructor

        public void addVertex(String lab)
        {
            vertexList[nVerts++] = new Vertex(lab);
        }
        public void addEdge(int start, int end, int weight)
        {
            adjMat[start, end] = weight;
            adjMat[end, start] = weight;
        }
        public void displayVertex(int v)
        {
            Console.Write(vertexList[v].harf_adı);
        }
        public void mstw() // minimum spanning tree
        {
            currentVert = 0; // start at 0
            while (nTree < nVerts - 1) // while not all verts in tree
            { // put currentVert in tree
                vertexList[currentVert].isInTree = true;
                nTree++;
                // insert edges adjacent to currentVert into PQ
                for (int j = 0; j < nVerts; j++) // for each vertex,
                {
                    if (j == currentVert) // skip if it’s us
                        continue;
                    if (vertexList[j].isInTree) // skip if in the tree
                        continue;
                    int distance = adjMat[currentVert, j];
                    if (distance == INFINITY) // skip if no edge
                        continue;
                    putInPQ(j, distance); // put it in PQ (maybe)
                }
                if (thePQ.getSize() == 0) // no vertices in PQ?
                {
                    Console.WriteLine(" GRAPH NOT CONNECTED");
                    return;
                }
                // remove edge with minimum distance, from PQ
                Edge theEdge = thePQ.removeMin();
                int sourceVert = theEdge.startVertex;
                currentVert = theEdge.endVertex;
                // display edge from source to current
                Console.Write(" " + vertexList[sourceVert].harf_adı);
                Console.Write(" " + vertexList[currentVert].harf_adı);
                Console.Write("  ");
            } // end while(not all verts in tree)
            // mst is complete
            for (int j = 0; j < nVerts; j++) // unmark vertices
                vertexList[j].isInTree = false;
        } // end mstw

        public void putInPQ(int newVert, int newDist)
        {
            // is there another edge with the same destination vertex?
            int queueIndex = thePQ.find(newVert);
            if (queueIndex != -1) // got edge’s index
            {
                Edge tempEdge = thePQ.peekN(queueIndex); // get edge
                int oldDist = tempEdge.distance;
                if (oldDist > newDist) // if new edge shorter,
                {
                    thePQ.removeN(queueIndex); // remove old edge
                    Edge theEdge =
                    new Edge(currentVert, newVert, newDist);
                    thePQ.insert(theEdge); // insert new edge
                }
                // else no action; just leave the old vertex there
            } // end if
            else // no edge with same destination vertex
            { // so insert new one
                Edge theEdge = new Edge(currentVert, newVert, newDist);
                thePQ.insert(theEdge);
            }
        } // end putInPQ()
        // -------------------------------------------------------------
    } // end class Graph

    class Program
    {
        static void Main(string[] args)
        {
            int boyut = 10;
            int[,] dizi = new int[boyut, boyut];
            int rastgele, x, y;
            Random r = new Random();
            Queue que = new Queue();
            int[] min_bulan_dizi = new int[boyut];

            Vertex[] vertex_list = new Vertex[boyut];
            Graph theGraph = new Graph();


            for (int i = 0; i < boyut; i++)        // random sayılar ile matrisime uzunluk değerleri atıyorum
            {
                for (int j = i + 1; j < boyut; j++)
                {
                    rastgele = r.Next(1, boyut);
                    if (i != j)                     // tabi eğer matrisin köşesinde değilsek
                    {
                        dizi[i, j] = rastgele;
                        dizi[j, i] = rastgele;
                        theGraph.addEdge(i, j, rastgele);
                    }
                    if (dizi[i, j] == 0 || dizi[j, i] == 0)
                    {
                        dizi[i, j] = Int32.MaxValue; //infinity
                        dizi[j, i] = Int32.MaxValue; //infinity
                    }
                }
                string going;
                switch (i)
                {
                    case 0:
                        going = "A";
                        vertex_list[0] = new Vertex(going);
                        theGraph.addVertex(going);
                        break;
                    case 1:
                        going = "B";
                        vertex_list[1] = new Vertex(going);
                        theGraph.addVertex(going);
                        break;
                    case 2:
                        going = "C";
                        vertex_list[2] = new Vertex(going);
                        theGraph.addVertex(going);
                        break;
                    case 3:
                        going = "D";
                        vertex_list[3] = new Vertex(going);
                        theGraph.addVertex(going);
                        break;
                    case 4:
                        going = "E";
                        vertex_list[4] = new Vertex(going);
                        theGraph.addVertex(going);
                        break;
                    case 5:
                        going = "F";
                        vertex_list[5] = new Vertex(going);
                        theGraph.addVertex(going);
                        break;
                    case 6:
                        going = "G";
                        vertex_list[6] = new Vertex(going);
                        theGraph.addVertex(going);
                        break;
                    case 7:
                        going = "H";
                        vertex_list[7] = new Vertex(going);
                        theGraph.addVertex(going);
                        break;
                    case 8:
                        going = "I";
                        vertex_list[8] = new Vertex(going);
                        theGraph.addVertex(going);
                        break;
                    case 9:
                        going = "J";
                        vertex_list[9] = new Vertex(going);
                        theGraph.addVertex(going);
                        break;
                }
            }


            Console.Write("Nereden ? ");        // kullanıcıdan iki integer değer alıyoruz
            x = (int)Convert.ToInt32(Console.ReadLine());       // nereden en kısa yol ile nereye gitmek istedikleri
            Console.Write("Nereye ? ");
            y = (int)Convert.ToInt32(Console.ReadLine());

            Dijkstra dijk = new Dijkstra(boyut, dizi, x, y);       // Dijkstra tipinde dijk değişkeni tanımlıyoruz

            for (int m = 0; m < boyut; m++)
            {
                Console.WriteLine("");
                for (int n = 0; n < boyut; n++)
                {
                        Console.Write(" " + dijk.distanceMat[m, n]);       // ve takip edilmesi kolay olsun diye tüm matrisi ekrana görüntületiyoruz
                }
            }

            dijk.run(que, min_bulan_dizi);        // Dijkstra's shortest path algoritmasını kullanarak en kısa yolu buluyoruz

            Console.Write("\n\nÇözüm: ");
            foreach (int i in dijk.D)
            {
                Console.Write(" "+i);   // nereden den tüm köşelere giden en kısa yollar
            }

            Console.WriteLine("\n\n " + x + "ten  " + y + "ye en kisa yol:  " + dijk.D[y - 1]);     // mutlu son =D

            que.Enqueue(y);
            Console.WriteLine("");
            foreach (object o in que)       // dijkstra'nın bulduğu en kısa yolda hangi yollardan geçtiğini ekrana yazdırıyoruz
            {
                if ((int)o != 0)
                    Console.WriteLine(" " + o + " ");
            }

            Console.WriteLine(" ");


            Console.WriteLine("Hangi tepeden genişlik öncelikli dolaşmaya başlamak istersiniz");
            int start = (int)Convert.ToInt16(Console.ReadLine());
            dijk.bfs(vertex_list, (start - 1));

            Console.Write("Minimum spanning tree: ");
            theGraph.mstw();        // minimum spanning tree
            Console.WriteLine();

            Console.WriteLine("Rapordaki şeklin bilgisayarda gerçekleştirimi");

            Graph gerceklesecek_graph = new Graph();
            
            string eklenecek_john = "John";
            gerceklesecek_graph.addVertex(eklenecek_john);
            string eklenecek_olivia = "Olivia";
            gerceklesecek_graph.addVertex(eklenecek_olivia);
            string eklenecek_celine = "Celine";
            gerceklesecek_graph.addVertex(eklenecek_celine);
            string eklenecek_jack = "Jack";
            gerceklesecek_graph.addVertex(eklenecek_jack);
            string eklenecek_chloe = "Chloe";
            gerceklesecek_graph.addVertex(eklenecek_chloe);
             string eklenecek_winston = "Winston";
            gerceklesecek_graph.addVertex(eklenecek_winston);
            
            Edge_other [] list_of_edge = new Edge_other[10];
            list_of_edge[0] = new Edge_other(eklenecek_john, eklenecek_olivia, 12);
            list_of_edge[1] = new Edge_other(eklenecek_olivia, eklenecek_celine, 5);
            list_of_edge[2] = new Edge_other(eklenecek_olivia, eklenecek_jack, 8);
            list_of_edge[3] = new Edge_other(eklenecek_john, eklenecek_jack, 7);
            list_of_edge[4] = new Edge_other(eklenecek_john, eklenecek_chloe, 9);
            list_of_edge[5] = new Edge_other(eklenecek_chloe, eklenecek_jack, 4);
            list_of_edge[6] = new Edge_other(eklenecek_chloe, eklenecek_winston, 15);
            list_of_edge[7] = new Edge_other(eklenecek_jack, eklenecek_winston, 16);
            list_of_edge[8] = new Edge_other(eklenecek_jack, eklenecek_celine, 6);
            list_of_edge[9] = new Edge_other(eklenecek_celine, eklenecek_winston, 10);

            Console.WriteLine("\n");
            String a, b;
            int d;
            for (int i = 0; i < 10; i++)
            {
                a = list_of_edge[i].vertex1;
                b = list_of_edge[i].vertex2;
                d = list_of_edge[i].distance;
                Console.WriteLine(" " + a + " ile " + b + " arasi : " + d + " kadar uzak.");
            }

            Console.ReadKey();

        }
    }
}