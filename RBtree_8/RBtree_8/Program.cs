using System;
using System.IO;
using System.Linq;
using System.Diagnostics;


internal class BinarySearchTree
{
	internal class Node
	{
		internal int data; // holds the key
		internal Node parent; // pointer to the parent
		internal Node left; // pointer to left child
		internal Node right; // pointer to right child
		internal int color; // 1 . Red, 0 . Black
	}


	// class RedBlackTree implements the operations in Red Black Tree
	public class RedBlackTree
	{
		private Node root;
		private Node TNULL;

		private void preOrderHelper(Node node)
		{
			if (node != TNULL)
			{
				Console.Write(node.data + " ");
				preOrderHelper(node.left);
				preOrderHelper(node.right);
			}
		}

		private void inOrderHelper(Node node)
		{
			if (node != TNULL)
			{
				inOrderHelper(node.left);
				Console.Write(node.data + " ");
				inOrderHelper(node.right);
			}
		}

		private void postOrderHelper(Node node)
		{
			if (node != TNULL)
			{
				postOrderHelper(node.left);
				postOrderHelper(node.right);
				Console.Write(node.data + " ");
			}
		}

		private Node searchTreeHelper(Node node, int key)
		{
			string sColor;
			//string sColor = root.color == 1 ? "RED" : "BLACK";
			if (node == TNULL )
			{
				Console.WriteLine("Пещера не найдена, копаем проход");
				//RB tree = new BinarySearchTree();
				//string Path = @"BST15.txt";
				//int[] array = { 0 };

				//array = File.ReadAllText(Path).Split().Select(int.Parse).ToArray();
				//for (int i = 0; i < array.Length; i++)
				//{
				//	tree.insert(array[i]);

				//}



				insert(key);
				searchTree(key);
				return null;
			}
			if (key == node.data)
            {
				sColor = node.color == 1 ? "RED" : "BLACK";
				Console.WriteLine("Пещера найдена, в ней: " + node.data+ "(" + sColor + ")" + " золота");
				return node;
			}


			if (key < node.data)
			{
				sColor = node.color == 1 ? "RED" : "BLACK";
				Console.WriteLine(node.data+ "("+ sColor+")");
				
				Console.WriteLine("Нужно идти в левый проход");
				return searchTreeHelper(node.left, key);
			}
			sColor = node.color == 1 ? "RED" : "BLACK";
			Console.WriteLine(node.data + "(" + sColor + ")");

			Console.WriteLine("Нужно идти в правый проход");
			return searchTreeHelper(node.right, key);
		}

		// fix the rb tree modified by the delete operation
		private void fixDelete(Node x)
		{
			Node s;
			while (x != root && x.color == 0)
			{
				if (x == x.parent.left)
				{
					s = x.parent.right;
					if (s.color == 1)
					{
						// case 3.1
						s.color = 0;
						x.parent.color = 1;
						leftRotate(x.parent);
						s = x.parent.right;
					}

					if (s.left.color == 0 && s.right.color == 0)
					{
						// case 3.2
						s.color = 1;
						x = x.parent;
					}
					else
					{
						if (s.right.color == 0)
						{
							// case 3.3
							s.left.color = 0;
							s.color = 1;
							rightRotate(s);
							s = x.parent.right;
						}

						// case 3.4
						s.color = x.parent.color;
						x.parent.color = 0;
						s.right.color = 0;
						leftRotate(x.parent);
						x = root;
					}
				}
				else
				{
					s = x.parent.left;
					if (s.color == 1)
					{
						// case 3.1
						s.color = 0;
						x.parent.color = 1;
						rightRotate(x.parent);
						s = x.parent.left;
					}

					if (s.right.color == 0 && s.right.color == 0)
					{
						// case 3.2
						s.color = 1;
						x = x.parent;
					}
					else
					{
						if (s.left.color == 0)
						{
							// case 3.3
							s.right.color = 0;
							s.color = 1;
							leftRotate(s);
							s = x.parent.left;
						}

						// case 3.4
						s.color = x.parent.color;
						x.parent.color = 0;
						s.left.color = 0;
						rightRotate(x.parent);
						x = root;
					}
				}
			}
			x.color = 0;
		}


		private void rbTransplant(Node u, Node v)
		{
			if (u.parent == null)
			{
				root = v;
			}
			else if (u == u.parent.left)
			{
				u.parent.left = v;
			}
			else
			{
				u.parent.right = v;
			}
			v.parent = u.parent;
		}

		private void deleteNodeHelper(Node node, int key)
		{
			// find the node containing key
			Node z = TNULL;
			Node x, y;
			while (node != TNULL)
			{
				if (node.data == key)
				{
					z = node;
				}

				if (node.data <= key)
				{
					node = node.right;
				}
				else
				{
					node = node.left;
				}
			}

			if (z == TNULL)
			{
				Console.WriteLine("Couldn't find key in the tree");
				return;
			}

			y = z;
			int yOriginalColor = y.color;
			if (z.left == TNULL)
			{
				x = z.right;
				rbTransplant(z, z.right);
			}
			else if (z.right == TNULL)
			{
				x = z.left;
				rbTransplant(z, z.left);
			}
			else
			{
				y = minimum(z.right);
				yOriginalColor = y.color;
				x = y.right;
				if (y.parent == z)
				{
					x.parent = y;
				}
				else
				{
					rbTransplant(y, y.right);
					y.right = z.right;
					y.right.parent = y;
				}

				rbTransplant(z, y);
				y.left = z.left;
				y.left.parent = y;
				y.color = z.color;
			}
			if (yOriginalColor == 0)
			{
				fixDelete(x);
			}
		}

		// fix the red-black tree
		private void fixInsert(Node k)
		{
			Node u;
			while (k.parent.color == 1)
			{
				if (k.parent == k.parent.parent.right)
				{
					u = k.parent.parent.left; // uncle
					if (u.color == 1)
					{
						// case 3.1
						u.color = 0;
						k.parent.color = 0;
						k.parent.parent.color = 1;
						k = k.parent.parent;
					}
					else
					{
						if (k == k.parent.left)
						{
							// case 3.2.2
							k = k.parent;
							rightRotate(k);
						}
						// case 3.2.1
						k.parent.color = 0;
						k.parent.parent.color = 1;
						leftRotate(k.parent.parent);
					}
				}
				else
				{
					u = k.parent.parent.right; // uncle

					if (u.color == 1)
					{
						// mirror case 3.1
						u.color = 0;
						k.parent.color = 0;
						k.parent.parent.color = 1;
						k = k.parent.parent;
					}
					else
					{
						if (k == k.parent.right)
						{
							// mirror case 3.2.2
							k = k.parent;
							leftRotate(k);
						}
						// mirror case 3.2.1
						k.parent.color = 0;
						k.parent.parent.color = 1;
						rightRotate(k.parent.parent);
					}
				}
				if (k == root)
				{
					break;
				}
			}
			root.color = 0;
		}

		private void printHelper(Node root, string indent, bool last)
		{
			// print the tree structure on the screen
			if (root != TNULL)
			{
				Console.Write(indent);
				if (last)
				{
					Console.Write("R----");
					indent += "     ";
				}
				else
				{
					Console.Write("L----");
					indent += "|    ";
				}

				string sColor = root.color == 1 ? "RED" : "BLACK";
				Console.WriteLine(root.data + "(" + sColor + ")");
				printHelper(root.left, indent, false);
				printHelper(root.right, indent, true);
			}
		}
		
		public RedBlackTree()
		{
			TNULL = new Node();
			TNULL.color = 0;
			TNULL.left = null;
			TNULL.right = null;
			root = TNULL;
		}

		// Pre-Order traversal
		// Node.Left Subtree.Right Subtree
		public virtual void preorder()
		{
			preOrderHelper(this.root);
		}

		// In-Order traversal
		// Left Subtree . Node . Right Subtree
		public virtual void inorder()
		{
			inOrderHelper(this.root);
		}

		// Post-Order traversal
		// Left Subtree . Right Subtree . Node
		public virtual void postorder()
		{
			postOrderHelper(this.root);
		}

		// search the tree for the key k
		// and return the corresponding node
		public virtual Node searchTree(int key)
		{
			return searchTreeHelper(this.root, key);
		}

		// find the node with the minimum key
		public virtual Node minimum(Node node)
		{
			while (node.left != TNULL)
			{
				node = node.left;
			}
			return node;
		}

		// find the node with the maximum key
		public virtual Node maximum(Node node)
		{
			while (node.right != TNULL)
			{
				node = node.right;
			}
			return node;
		}

		// find the successor of a given node
		public virtual Node successor(Node x)
		{
			// if the right subtree is not null,
			// the successor is the leftmost node in the
			// right subtree
			if (x.right != TNULL)
			{
				return minimum(x.right);
			}

			// else it is the lowest ancestor of x whose
			// left child is also an ancestor of x.
			Node y = x.parent;
			while (y != TNULL && x == y.right)
			{
				x = y;
				y = y.parent;
			}
			return y;
		}

		// find the predecessor of a given node
		public virtual Node predecessor(Node x)
		{
			// if the left subtree is not null,
			// the predecessor is the rightmost node in the 
			// left subtree
			if (x.left != TNULL)
			{
				return maximum(x.left);
			}

			Node y = x.parent;
			while (y != TNULL && x == y.left)
			{
				x = y;
				y = y.parent;
			}

			return y;
		}

		// rotate left at node x
		public virtual void leftRotate(Node x)
		{
			Node y = x.right;
			x.right = y.left;
			if (y.left != TNULL)
			{
				y.left.parent = x;
			}
			y.parent = x.parent;
			if (x.parent == null)
			{
				this.root = y;
			}
			else if (x == x.parent.left)
			{
				x.parent.left = y;
			}
			else
			{
				x.parent.right = y;
			}
			y.left = x;
			x.parent = y;
		}

		// rotate right at node x
		public virtual void rightRotate(Node x)
		{
			Node y = x.left;
			x.left = y.right;
			if (y.right != TNULL)
			{
				y.right.parent = x;
			}
			y.parent = x.parent;
			if (x.parent == null)
			{
				this.root = y;
			}
			else if (x == x.parent.right)
			{
				x.parent.right = y;
			}
			else
			{
				x.parent.left = y;
			}
			y.right = x;
			x.parent = y;
		}

		// insert the key to the tree in its appropriate position
		// and fix the tree
		public virtual void insert(int key)
		{
			// Ordinary Binary Search Insertion
			Node node = new Node();
			node.parent = null;
			node.data = key;
			node.left = TNULL;
			node.right = TNULL;
			node.color = 1; // new node must be red

			Node y = null;
			Node x = this.root;

			while (x != TNULL)
			{
				y = x;
				if (node.data < x.data)
				{
					x = x.left;
				}
				else
				{
					x = x.right;
				}
			}

			// y is parent of x
			node.parent = y;
			if (y == null)
			{
				root = node;
			}
			else if (node.data < y.data)
			{
				y.left = node;
			}
			else
			{
				y.right = node;
			}

			// if new node is a root node, simply return
			if (node.parent == null)
			{
				node.color = 0;
				return;
			}

			// if the grandparent is null, simply return
			if (node.parent.parent == null)
			{
				return;
			}

			// Fix the tree
			fixInsert(node);
		}

		public virtual Node Root
		{
			get
			{
				return this.root;
			}
		}

		// delete the node from the tree
		public virtual void deleteNode(int data)
		{
			Console.WriteLine("Происходит обвал пещеры: " + data);
			deleteNodeHelper(this.root, data);
		}
		public virtual void prettyPrint()
		{
			printHelper(this.root, "", true);
		}

		public static void Main(string[] args)
		{
			RedBlackTree tree = new RedBlackTree();
			Console.WriteLine("Введите количество золота: ");
			int search = Convert.ToInt32(Console.ReadLine());
			Console.WriteLine("Введите пещеру для обвала: ");
			int delete = Convert.ToInt32(Console.ReadLine());


			

			string Path = @"BST15.txt";
			int[] array = { 0 };

			array = File.ReadAllText(Path).Split().Select(int.Parse).ToArray();




			for (int i = 0; i < array.Length; i++)
			{
				tree.insert(array[i]);

			}
			//tree.prettyPrint();
			//int MaxIndex = array.Length+2;

			//tree.BalanceTree();
			if (array.Contains(delete))
            {
                tree.deleteNode(delete);
            }
            else
            {
                Console.WriteLine("Среди пещер нет пещеры " + delete);

            }
            if (search != delete)
			{
				tree.searchTree(search);
			}
			else
			{
				Console.WriteLine("Введенная пещера не может быть найдена, в ней произошел обвал");
			}
			//tree.prettyPrint();
			//bst.insert(8);
			//bst.insert(18);
			//bst.insert(5);
			//bst.insert(15);
			//bst.insert(17);
			//bst.insert(25);
			//bst.insert(40);
			//bst.insert(80);
			//bst.deleteNode(25);
			//bst.prettyPrint();
		}
	}




}



