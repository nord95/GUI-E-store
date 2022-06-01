using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;



namespace Projektarbete
{
    public class Product
    {
        public string ImagePath { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        private double price { get; set; }
        public double Price
        {
            get
            {
                return price;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Price can't be negative.");
                }
                else
                {
                    price = value;
                }
            }
        }
    }
    public class Voucher
    {
        private int discount { get; set; }

        public int Discount
        {
            get
            {
                return discount;
            }
            set
            {
                if (value > 100)
                {
                    throw new ArgumentException("you cant have more than 100% discount");
                }
                else
                {
                    discount = value;
                }
            }
        }
        private string code { get; set; }
        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                int count = 0;
                foreach (char c in value)
                {

                    count++;

                    if (!Char.IsLetterOrDigit(c))
                    {
                        throw new ArgumentException("Voucher may contain only letters and numbers");
                    }
                }
                if (count > 20)
                {
                    throw new ArgumentException("Voucher is to long");
                }
                else if (count < 3)
                {
                    throw new ArgumentException("Voucher is to short");
                }
                else
                {
                    code = value;
                }
            }
        }
    }
    public partial class MainWindow : Window
    {
        //instance variables
        private string productFilePath = "Products.csv";
        public static string SaveProductPath = @"C:\Windows\Temp\AllProduct.csv";
        private Product[] products;
        private List<Product> listCart = new List<Product>();

        private string cartFilePath = @"C:\Windows\Temp\Cart.csv";
        private Dictionary<Product, int> cart = new Dictionary<Product, int>();

        public static string ImageFileTemp = @"C:\Windows\Temp\";
        public static string ImageMap = @"Pictures\";

        private string voucherFilePath = "Voucher.csv";
        public static string SaveVoucherPath = @"C:\Windows\Temp\Voucher.csv";
        private List<Voucher> vouchers = new List<Voucher>();
        private string usedVoucherString;

        private StackPanel productPanel;
        private StackPanel lowerCartPanel;
        private StackPanel topCartPanel;
        private Grid mainGrid;
        private Grid newProductGrid;
        private Grid gridInPanel;

        private int usedVoucher;
        private TextBox inputVoucher;
        private int selectedQuantity;
        private double sum;
        private double voucherDouble;
        private ComboBox amountToCart;
        private Label newProduct;

        public MainWindow()
        {
            InitializeComponent();
            Start();
        }

        private void Start()
        {
            // Window options
            Title = "Store";
            Width = 700;
            Height = 620;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            // Scrolling
            ScrollViewer root = new ScrollViewer();
            root.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            Content = root;

            // Main grid
            mainGrid = new Grid();
            root.Content = mainGrid;
            mainGrid.Margin = new Thickness(5);
            mainGrid.RowDefinitions.Add(new RowDefinition());
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition());

            //all the available products will be stacked in this stackpanel
            productPanel = new StackPanel
            {
                Background = Brushes.LightSteelBlue
            };
            mainGrid.Children.Add(productPanel);
            Grid.SetColumn(productPanel, 0);

            Grid cartgrid = new Grid();
            cartgrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            cartgrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            cartgrid.RowDefinitions.Add(new RowDefinition());
            cartgrid.ColumnDefinitions.Add(new ColumnDefinition());
            mainGrid.Children.Add(cartgrid);
            Grid.SetRow(cartgrid, 0);
            Grid.SetColumn(cartgrid, 1);

            //added products in the cart will be stacked here
            topCartPanel = new StackPanel
            {
                Background = Brushes.LightGray
            };
            cartgrid.Children.Add(topCartPanel);
            Grid.SetColumn(topCartPanel, 0);
            Grid.SetRow(topCartPanel, 1);

            //all buttons associated with Cart will be stacked here
            lowerCartPanel = new StackPanel
            {
                Background = Brushes.CadetBlue
            };
            cartgrid.Children.Add(lowerCartPanel);
            Grid.SetColumn(lowerCartPanel, 0);
            Grid.SetRow(lowerCartPanel, 2);

            Label headLabelCart = NewLabel("Cart", cartgrid, 0, 0);
            headLabelCart.Margin = new Thickness(0);
            headLabelCart.FontSize = 20;
            headLabelCart.FontFamily = new FontFamily("Bold");
            headLabelCart.Background = Brushes.LightGray;
            headLabelCart.HorizontalContentAlignment = HorizontalAlignment.Center;

            Label headLabelProduct = NewLabel("Products", productPanel);
            headLabelProduct.FontSize = 20;
            headLabelProduct.FontFamily = new FontFamily("Bold");
            headLabelProduct.HorizontalContentAlignment = HorizontalAlignment.Center;

            //if the predefined or saved vouchers fits the requirements add to vouchers list
            if (File.Exists(SaveVoucherPath))
            {
                vouchers = TryVoucherCode(SaveVoucherPath);
            }
            else
            {
                vouchers = TryVoucherCode(voucherFilePath);
            }
            //copy predefined vouchers and products to a new tempfile if the file dont exsist
            try
            {
                File.Copy(voucherFilePath, SaveVoucherPath);
            }
            catch { }
            try
            {
                File.Copy(productFilePath, SaveProductPath);
            }
            catch { }

            PrintProduct(productPanel);
            //copy every predefined image to tempfile
            foreach (Product p in products)
            {
                string imagepath = p.ImagePath;
                try
                {
                    File.Copy(ImageMap + imagepath, ImageFileTemp + imagepath);
                }
                catch
                {

                }
            }
            //if there is a saved cart, print the products
            if (File.Exists(cartFilePath))
            {
                PrintSavedProducts();
            }

            Label voucher = NewLabel("discount code:", lowerCartPanel);
            voucher.FontSize = 15;

            Grid voucherGrid = new Grid();
            voucherGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            voucherGrid.ColumnDefinitions.Add(new ColumnDefinition());
            voucherGrid.ColumnDefinitions.Add(new ColumnDefinition());
            lowerCartPanel.Children.Add(voucherGrid);

            inputVoucher = new TextBox
            {
                Background = Brushes.White,
                Margin = new Thickness(5),
                Padding = new Thickness(5),
            };
            voucherGrid.Children.Add(inputVoucher);
            Grid.SetRow(inputVoucher, 0);
            Grid.SetColumn(inputVoucher, 0);

            Button tryVoucherButton = AddButton("Try voucher", voucherGrid, 0, 1);
            //click or press enter
            tryVoucherButton.Click += TryVoucherButton_Click;
            tryVoucherButton.IsDefault = true;

            Button deleteCartButton = AddButton("Delete cart", lowerCartPanel);
            deleteCartButton.Tag = topCartPanel;
            deleteCartButton.Click += DeleteCartButton_Click;

            Button orderButton = AddButton("Order", lowerCartPanel);
            orderButton.Click += OrderButton_Click;

            Button saveCartButton = AddButton("Save cart", lowerCartPanel);
            saveCartButton.Click += SaveCartButton_Click;
        }

        //trys if the input voucher match one of the predefined
        private void TryVoucherButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            var stringLines = File.ReadAllLines(SaveVoucherPath);
            //do this if no voucher has been used
            if (usedVoucher < 1)
            {
                foreach (var line in stringLines)
                {
                    // If the text box is empty
                    if (inputVoucher.Text.Trim().ToLower() == "")
                    {
                        return;
                    }
                    //if the code before the',' in the line are equals to the input
                    else if (line.Split(',')[0].Equals(inputVoucher.Text.ToLower().Trim()))
                    {
                        usedVoucherString = inputVoucher.Text.ToLower().Trim();
                        usedVoucher++;
                        inputVoucher.Background = Brushes.LightGreen;
                        // If the note has already been added, show a message and clear the text box.
                        MessageBox.Show("Succesful voucher input");

                        string voucherCode = line.Split(',')[0];

                        voucherDouble = double.Parse(line.Split(',')[1]);

                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("Already used a voucher");
            }
        }

        private void SaveCartButton_Click(object sender, RoutedEventArgs e)
        {
            //loops through the products in the cart and create a string to each product
            List<string> lines = new List<string>();
            foreach (KeyValuePair<Product, int> pair in cart)
            {
                Product p = pair.Key;
                selectedQuantity = pair.Value;

                lines.Add(p.Name + "," + p.Price + "," + selectedQuantity);
            }
            //saves the list in a .csv file in Temp
            File.WriteAllLines(cartFilePath, lines);

            MessageBox.Show("your products in the cart are saved");
        }
        private void DeleteCartButton_Click(object sender, RoutedEventArgs e)
        {
            Button deleteCart = (Button)sender;
            StackPanel productInCart = (StackPanel)deleteCart.Tag;
            productInCart.Children.Clear();
            File.Delete(cartFilePath);
            cart.Clear();
            listCart.Clear();
        }
        //clear the stackpanel and print the current products in the cart
        private void AmountToCart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            amountToCart = (ComboBox)sender;
            Product product = (Product)amountToCart.Tag;
            //the selectet quantity
            int index = amountToCart.SelectedIndex;
            selectedQuantity = int.Parse(amountToCart.SelectedItem.ToString());

            if (selectedQuantity != 0)
            {
                //if cart dont contain this product, add to cart and listcart
                if (!cart.ContainsKey(product))
                {
                    cart.Add(product, selectedQuantity);
                    listCart.Add(product);
                    topCartPanel.Children.Clear();
                    selectedQuantity = 0;
                    amountToCart.SelectedIndex = 0;
                }
                //if cart already contains this product, add selected quantity to the previous quantity
                else if (cart.ContainsKey(product))
                {
                    cart[product] += selectedQuantity;
                    topCartPanel.Children.Clear();
                    selectedQuantity = 0;
                    amountToCart.SelectedIndex = 0;
                }
                //create a new grid to each product 
                foreach (KeyValuePair<Product, int> pair in cart)
                {
                    Product p = pair.Key;
                    selectedQuantity = pair.Value;

                    newProductGrid = new Grid();
                    newProductGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    newProductGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    newProductGrid.RowDefinitions.Add(new RowDefinition());
                    topCartPanel.Children.Add(newProductGrid);

                    newProduct = new Label
                    {
                        Content = $"{p.Name}, {p.Price}kr/piece, \n" +
                        $" qty: {cart[p]}, (total {p.Price * cart[p]} kr)",
                    };

                    newProductGrid.Children.Add(newProduct);
                    Grid.SetRow(newProduct, 0);
                    Grid.SetColumn(newProduct, 0);

                    Button deleteButton = AddButton("Delete", newProductGrid, 0, 1);
                    deleteButton.Tag = p;
                    deleteButton.Click += DeleteButton_Click;

                    selectedQuantity = 0;
                    amountToCart.SelectedIndex = 0;
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Product product = (Product)button.Tag;

            int indexToDelete = listCart.IndexOf(product);

            // Remove from the GUI,dictionary and list
            topCartPanel.Children.RemoveAt(indexToDelete);
            cart.Remove(product);
            listCart.RemoveAt(indexToDelete);

            if (cart.Count == 0)
            {
                File.Delete(cartFilePath);
            }
        }
        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {   //if the cart is empty you cant order
            if (cart.Count == 0)
            {
                MessageBox.Show("there is nothing in the shopping cart ");
            }
            else
            {
                //clear everything and create a new grid and print the receipt
                mainGrid.Children.Clear();

                Title = "Receipt";
                Width = 400;
                Height = 400;
                WindowStartupLocation = WindowStartupLocation.CenterScreen;

                ScrollViewer root = new ScrollViewer();
                root.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                Content = root;

                mainGrid = new Grid();
                root.Content = mainGrid;
                mainGrid.Margin = new Thickness(5);
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
                mainGrid.RowDefinitions.Add(new RowDefinition());

                StackPanel receipt = new StackPanel
                {
                    Background = Brushes.LightGray
                };
                mainGrid.Children.Add(receipt);

                Label headlineReceipt = NewLabel("RECEIPT", receipt);
                headlineReceipt.FontSize = 20;
                headlineReceipt.HorizontalContentAlignment = HorizontalAlignment.Center;

                Grid receiptHeadlineGrid = new Grid();
                receiptHeadlineGrid.ColumnDefinitions.Add(new ColumnDefinition());
                receiptHeadlineGrid.ColumnDefinitions.Add(new ColumnDefinition());
                receiptHeadlineGrid.ColumnDefinitions.Add(new ColumnDefinition());
                receiptHeadlineGrid.ColumnDefinitions.Add(new ColumnDefinition());
                receiptHeadlineGrid.RowDefinitions.Add(new RowDefinition());
                receipt.Children.Add(receiptHeadlineGrid);

                Label headlineTitel = NewLabel("titel", receiptHeadlineGrid, 0, 0);

                Label headlineQuantity = NewLabel("quantity", receiptHeadlineGrid, 0, 1);

                Label headlinePrice = NewLabel("price/qty", receiptHeadlineGrid, 0, 2);

                Label headlineTotal = NewLabel("total", receiptHeadlineGrid, 0, 3);

                //loop through cart and create a grid with info to print
                foreach (KeyValuePair<Product, int> pair in cart)
                {
                    Grid productReceiptGrid = new Grid();
                    productReceiptGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    productReceiptGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    productReceiptGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    productReceiptGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    productReceiptGrid.RowDefinitions.Add(new RowDefinition());
                    receipt.Children.Add(productReceiptGrid);

                    Product p = pair.Key;
                    selectedQuantity = pair.Value;
                    string name = p.Name;
                    double price = Math.Round((p.Price), 2);
                    double total = Math.Round((selectedQuantity * price), 2);
                    sum += total;

                    Label productTitle = NewLabel(name, productReceiptGrid, 0, 0);

                    Label productQuantity = NewLabel(selectedQuantity.ToString(), productReceiptGrid, 0, 1);

                    Label productPrice = NewLabel(price.ToString(), productReceiptGrid, 0, 2);

                    Label productTotal = NewLabel(total.ToString(), productReceiptGrid, 0, 3);

                }
                double discountSum = Math.Round(((voucherDouble / 100) * sum), 2);
                //calculation takes place in a method to be able to test the result
                double sumToPay = SumAfterDiscount(voucherDouble, sum);

                //a grid to print added values, results from the loop
                Grid bottomOfReceiptGrid = new Grid();
                bottomOfReceiptGrid.ColumnDefinitions.Add(new ColumnDefinition());
                bottomOfReceiptGrid.ColumnDefinitions.Add(new ColumnDefinition());
                bottomOfReceiptGrid.RowDefinitions.Add(new RowDefinition());
                bottomOfReceiptGrid.RowDefinitions.Add(new RowDefinition());
                bottomOfReceiptGrid.RowDefinitions.Add(new RowDefinition());
                bottomOfReceiptGrid.RowDefinitions.Add(new RowDefinition());
                bottomOfReceiptGrid.Background = Brushes.GhostWhite;

                bottomOfReceiptGrid.Margin = new Thickness(5);
                receipt.Children.Add(bottomOfReceiptGrid);

                Label sumBeforeVoucher = NewLabel("Sum: ", bottomOfReceiptGrid, 0, 0);
                Label inputSum = NewLabel(Math.Round(sum, 2).ToString(), bottomOfReceiptGrid, 0, 1);

                Label voucherCode = NewLabel("Voucher code: ", bottomOfReceiptGrid, 1, 0);
                Label choosenCode = NewLabel(usedVoucherString, bottomOfReceiptGrid, 1, 1);

                Label disscount = NewLabel("Discount: ", bottomOfReceiptGrid, 2, 0);
                Label discount = NewLabel(discountSum.ToString(), bottomOfReceiptGrid, 2, 1);

                Label sumAfterVoucher = NewLabel("Sum after discount: ", bottomOfReceiptGrid, 3, 0);
                Label finalSum = NewLabel(sumToPay.ToString(), bottomOfReceiptGrid, 3, 1);
                sumAfterVoucher.FontFamily = new FontFamily("Segoe UI Bold");

                File.Delete(cartFilePath);
                cart.Clear();
                listCart.Clear();
            }
        }
        private static Label NewLabel(string content, StackPanel panel)
        {
            Label label = new Label
            {
                Margin = new Thickness(5),
                Padding = new Thickness(5),
                Content = content,
                FontFamily = new FontFamily("Segoe UI")
            };
            panel.Children.Add(label);
            return label;
        }
        private Label NewLabel(string content, Grid grid, int row, int column)
        {
            Label label = new Label
            {
                Margin = new Thickness(5),
                Padding = new Thickness(5),
                Content = content,
                FontFamily = new FontFamily("Segoe UI")
            };
            grid.Children.Add(label);
            Grid.SetRow(label, row);
            Grid.SetColumn(label, column);

            return label;
        }
        public static Button AddButton(string content, StackPanel panel)
        {
            Button add = new Button
            {
                Margin = new Thickness(5),
                Padding = new Thickness(5),
                Content = content,
            };
            panel.Children.Add(add);

            return add;
        }

        public Button AddButton(string content, Grid grid, int row, int column)
        {
            Button add = new Button
            {
                Margin = new Thickness(5),
                Padding = new Thickness(5),
                Content = content
            };
            grid.Children.Add(add);
            Grid.SetColumn(add, column);
            Grid.SetRow(add, row);

            return add;
        }
        //create a image from the selected images filpath
        public static Image CreateImage(string filePath)
        {
            ImageSource source = new BitmapImage(new Uri(filePath, UriKind.RelativeOrAbsolute));
            Image image = new Image
            {
                Source = source,
                Width = 100,
                Height = 100,
                Stretch = Stretch.UniformToFill,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5)
            };
            // A small rendering tweak to ensure maximum visual appeal.
            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
            return image;
        }
        //reads all lines from Products.csv file
        public static Product[] ReadProducts(string filePath)
        {
            //save all lines in array "lines"
            List<Product> products = new List<Product>();
            string[] lines = File.ReadAllLines(filePath);

            try
            {
                //loop through all lines and split the string on commas, saves the split string in an array
                //returns all the new products in a list whit Product
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');

                    Product service = new Product
                    {
                        ImagePath = parts[0],
                        Name = parts[1],
                        Description = parts[2],
                        Price = double.Parse(parts[3])
                    };

                    products.Add(service);
                }
            }
            catch
            {
                MessageBox.Show("all products can't be added.\n" +
                "some information about the product is incorrect");
            }
            return products.ToArray();
        }
        //loading the saved product from Cart.csv in Tempfile to the dictionary "cart"
        public Dictionary<Product, int> ReadSavedCart(string filepath)
        {
            Dictionary<Product, int> savedCart = new Dictionary<Product, int>();
            string[] lines = File.ReadAllLines(filepath);
            //go through all lines, split on commas.
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');

                string name = parts[0];
                selectedQuantity = int.Parse(parts[2]);
                Product thisProduct = null;
                //loop through the list with all the products, when the name matches,
                //enter all information about the product
                foreach (Product p in products)
                {
                    if (p.Name == name)
                    {
                        thisProduct = p;
                    }
                }
                //dictionary key (product) gets the associated value(quantity)
                cart[thisProduct] = selectedQuantity;
            }
            return cart;
        }
        //prints all the saved products in dictionary cart and add to a list of all the products in the cart 
        public void PrintSavedProducts()
        {
            cart = ReadSavedCart(cartFilePath);
            foreach (KeyValuePair<Product, int> pair in cart)
            {
                Product p = pair.Key;
                selectedQuantity = pair.Value;
                listCart.Add(p);

                newProductGrid = new Grid();
                newProductGrid.ColumnDefinitions.Add(new ColumnDefinition());
                newProductGrid.ColumnDefinitions.Add(new ColumnDefinition());
                newProductGrid.RowDefinitions.Add(new RowDefinition());
                topCartPanel.Children.Add(newProductGrid);

                newProduct = new Label
                {
                    Content = $"{p.Name}, {p.Price}kr/piece, \n" +
                    $" qty: {selectedQuantity}, (total {p.Price * selectedQuantity} kr)",
                    Tag = p
                };

                newProductGrid.Children.Add(newProduct);
                Grid.SetRow(newProduct, 0);
                Grid.SetColumn(newProduct, 0);

                Button deleteButton = AddButton("Delete", newProductGrid, 0, 1);
                deleteButton.Tag = p;
                deleteButton.Click += DeleteButton_Click;

                selectedQuantity = 0;
                amountToCart.SelectedIndex = 0;
            }
        }

        public static double SumAfterDiscount(double percentage, double sum)
        {
            double sumToPay = Math.Round((((100 - percentage) / 100) * sum), 2);
            return sumToPay;
        }
        //reads and print all the products listed in "products"
        public void PrintProduct(StackPanel panel)
        {
            //read the products from file and save them in "products"
            products = ReadProducts(SaveProductPath);

            //create a layout to each product.
            foreach (Product product in products)
            {
                Image uniformImage = CreateImage(ImageMap + product.ImagePath);
                if (File.Exists(ImageFileTemp + product.ImagePath))
                {
                    uniformImage = CreateImage(ImageFileTemp + product.ImagePath);
                }
                uniformImage.Stretch = Stretch.Uniform;
                panel.Children.Add(uniformImage);
                Grid.SetRow(uniformImage, 0);
                Grid.SetColumn(uniformImage, 0);

                gridInPanel = new Grid();

                gridInPanel.RowDefinitions.Add(new RowDefinition());
                gridInPanel.RowDefinitions.Add(new RowDefinition());
                gridInPanel.RowDefinitions.Add(new RowDefinition());
                gridInPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.5, GridUnitType.Star) });
                gridInPanel.ColumnDefinitions.Add(new ColumnDefinition());
                panel.Children.Add(gridInPanel);

                string printText = $"{product.Name}, {product.Description}, {product.Price} kr ";

                TextBox productInfo = new TextBox
                {
                    Text = printText,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(5),
                    FontFamily = new FontFamily("Segoe UI Italic"),
                    FontSize = 14,
                    TextAlignment = TextAlignment.Center,
                    BorderBrush = Brushes.Transparent,
                    Background = Brushes.Transparent,

                };
                gridInPanel.Children.Add(productInfo);
                Grid.SetColumn(productInfo, 0);
                Grid.SetRow(productInfo, 1);
                Grid.SetColumnSpan(productInfo, 2);

                amountToCart = new ComboBox
                {
                    Margin = new Thickness(5),
                    Padding = new Thickness(5),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Tag = product
                };
                amountToCart.SelectedIndex = 0;
                amountToCart.SelectionChanged += AmountToCart_SelectionChanged;

                for (int i = 0; i <= 100; i++)
                {
                    amountToCart.Items.Add(i);
                }
                gridInPanel.Children.Add(amountToCart);
                Grid.SetColumn(amountToCart, 1);
                Grid.SetRow(amountToCart, 2);

                Label addLabel = NewLabel("Quantity to add:", gridInPanel, 2, 0);
                addLabel.Background = Brushes.Gainsboro;
                addLabel.BorderBrush = Brushes.LightSlateGray;
                addLabel.BorderThickness = new Thickness(1);
                addLabel.HorizontalAlignment = HorizontalAlignment.Right;
                addLabel.FontFamily = new FontFamily("Segoe UI Bold");
            }
        }
        //try the predefined vouchers
        public static List<Voucher> TryVoucherCode(string filepath)
        {
            var voucherList = new List<Voucher> { };
            var stringLines = File.ReadAllLines(filepath);
            try
            {
                foreach (var line in stringLines)
                {
                    string[] parts = line.Split(',');

                    Voucher vouchercode = new Voucher
                    {
                        Code = parts[0],
                        Discount = int.Parse(parts[1]),
                    };
                    voucherList.Add(vouchercode);
                }
            }
            catch
            {
                MessageBox.Show("all voucher codes cannot be added, check the lenght or the discont");
            }
            return voucherList;
        }

    }
}