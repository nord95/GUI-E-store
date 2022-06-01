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
using Projektarbete;
using System.IO;

namespace vgDelen
{
    public partial class MainWindow : Window
    {
        //instance variables
        List<Product> allProducts = new List<Product> { };
        List<String> allProductLines;
        public static Product p = new Product { };

        List<Voucher> allVouchers = new List<Voucher> { };
        List<string> allVoucherLines;
        Voucher v = new Voucher { };
        string voucherString="";

        public static string SavedProductPath = @"C:\Windows\Temp\AllProduct.csv";
        public static string SavedImagesPath = @"C:\Windows\Temp\";
        public static string SavedVoucherPath = @"C:\Windows\Temp\Voucher.csv";

        StackPanel showProductPanel;
        StackPanel showVoucherPanel;
        Grid oneProductGrid;
        Grid createProduct;
        Grid newProductGrid;

        TextBox nameTextBox;
        TextBox descriptionBox;
        TextBox pricebox;
        TextBox voucherBox;
        TextBox precentageBox;
        TextBox descriptionProduct;
        TextBox nameProduct;
        TextBox priceProduct;
        TextBox voucherCode;
        TextBox voucherPrecent;

        public MainWindow()
        {
            InitializeComponent();
            Start();
        }
        private void Start()
        {
            // Window options
            Title = "GUI App";
            Width = 800;
            Height = 500;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            // Scrolling
            ScrollViewer root = new ScrollViewer();
            root.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            Content = root;

            // Main grid
            Grid grid = new Grid();
            root.Content = grid;
            grid.Margin = new Thickness(5);
            grid.RowDefinitions.Add(new RowDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.Background = Brushes.CadetBlue;

            //left side where you create new products and vouchers
            createProduct = new Grid();
            createProduct.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            createProduct.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            createProduct.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            createProduct.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            createProduct.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            createProduct.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            createProduct.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            createProduct.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            createProduct.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            createProduct.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            createProduct.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            createProduct.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            createProduct.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            createProduct.ColumnDefinitions.Add(new ColumnDefinition());
            createProduct.ColumnDefinitions.Add(new ColumnDefinition());
            createProduct.Background = Brushes.LightSteelBlue;
            grid.Children.Add(createProduct);

            showProductPanel = new StackPanel();
            grid.Children.Add(showProductPanel);
            Grid.SetColumn(showProductPanel, 1);

            showVoucherPanel = new StackPanel
            {
                Background = Brushes.LightGray
            };
            createProduct.Children.Add(showVoucherPanel);
            Grid.SetColumnSpan(showVoucherPanel, 2);
            Grid.SetRow(showVoucherPanel, 13);

            oneProductGrid = new Grid();
            oneProductGrid.RowDefinitions.Add(new RowDefinition());
            oneProductGrid.RowDefinitions.Add(new RowDefinition());
            oneProductGrid.ColumnDefinitions.Add(new ColumnDefinition());
            oneProductGrid.ColumnDefinitions.Add(new ColumnDefinition());
            oneProductGrid.ColumnDefinitions.Add(new ColumnDefinition());
            showProductPanel.Children.Add(oneProductGrid);

            // Loading and printing existing products, to and from the list
            if (File.Exists(SavedProductPath))
            {
                allProducts = ReadProducts(SavedProductPath);
            }
            Printproducts(allProducts);

            // Loading and printing existing vouchers, to and from the list
            if (File.Exists(SavedVoucherPath))
            {
                allVouchers = ReadVouchers(SavedVoucherPath);
            }
            PrintVouchers(allVouchers);

            Label createProductLabel = new Label
            {
                Content = "Create a new product:",
                FontSize = 15,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };
            createProduct.Children.Add(createProductLabel);
            Grid.SetColumnSpan(createProductLabel, 2);

            Label nameLabel = new Label
            {
                Content = "Name of the product:",
            };
            createProduct.Children.Add(nameLabel);
            Grid.SetColumn(nameLabel, 0);
            Grid.SetRow(nameLabel, 1);

            nameTextBox = new TextBox
            {
                Tag = p
            };
            createProduct.Children.Add(nameTextBox);
            Grid.SetRow(nameTextBox, 1);
            Grid.SetColumn(nameTextBox, 1);
            nameTextBox.TextChanged += NewName_TextChanged;

            Label descriptionLabel = new Label
            {
                Content = "Description of the product:",
            };
            createProduct.Children.Add(descriptionLabel);
            Grid.SetColumn(descriptionLabel, 0);
            Grid.SetRow(descriptionLabel, 2);

            descriptionBox = new TextBox
            {
                Tag = p
            };
            createProduct.Children.Add(descriptionBox);
            Grid.SetRow(descriptionBox, 2);
            Grid.SetColumn(descriptionBox, 1);
            descriptionBox.TextChanged += NewDescription_TextChanged;

            Label priceLabel = new Label
            {
                Content = "Price of the product"
            };
            createProduct.Children.Add(priceLabel);
            Grid.SetColumn(priceLabel, 0);
            Grid.SetRow(priceLabel, 3);

            pricebox = new TextBox
            {
                Tag = p
            };
            createProduct.Children.Add(pricebox);
            Grid.SetRow(pricebox, 3);
            Grid.SetColumn(pricebox, 1);
            pricebox.TextChanged += NewPrice_TextChanged;

            Button addImageButton = new Button
            {
                Content = "add image of the product",
                Tag = p,
                Margin = new Thickness(5),
                Padding = new Thickness(5)
            };
            createProduct.Children.Add(addImageButton);
            Grid.SetColumnSpan(addImageButton, 2);
            Grid.SetRow(addImageButton, 4);
            addImageButton.Click += AddImageButton_Click;

            Label createVoucherLabel = new Label
            {
                Content = "\nCreate a new voucher:",
                FontSize = 15,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };
            createProduct.Children.Add(createVoucherLabel);
            Grid.SetColumnSpan(createVoucherLabel, 2);
            Grid.SetRow(createVoucherLabel, 7);
            Grid.SetColumn(createVoucherLabel, 0);

            Label voucherLabel = new Label
            {
                Content = "vouchercode: "
            };
            createProduct.Children.Add(voucherLabel);
            Grid.SetColumn(voucherLabel, 0);
            Grid.SetRow(voucherLabel, 8);

            voucherBox = new TextBox
            {
                Tag = v
            };
            createProduct.Children.Add(voucherBox);
            Grid.SetRow(voucherBox, 8);
            Grid.SetColumn(voucherBox, 1);
            voucherBox.TextChanged += NewCodeBox_TextChanged;

            Label percentageLabel = new Label
            {
                Content = "Percentage: "
            };
            createProduct.Children.Add(percentageLabel);
            Grid.SetColumn(percentageLabel, 0);
            Grid.SetRow(percentageLabel, 9);

            precentageBox = new TextBox
            {
                Tag = v
            };
            createProduct.Children.Add(precentageBox);
            Grid.SetRow(precentageBox, 9);
            Grid.SetColumn(precentageBox, 1);
            precentageBox.TextChanged += NewPrecentageBox_TextChanged;

            Button addVoucherButton = new Button
            {
                Content = "Add new voucher",
                Tag = v,
                Margin = new Thickness(5),
                Padding = new Thickness(5)
            };
            createProduct.Children.Add(addVoucherButton);
            Grid.SetColumnSpan(addVoucherButton, 2);
            Grid.SetRow(addVoucherButton, 10);
            addVoucherButton.Click += AddNewVoucherButton_Click;
        }
        //takes the saved inputs from text box and create a new Product and saves it in allProduct list
        //and prints it.
        private void AddNewProductButton_Click(object sender, RoutedEventArgs e)
        {
            Button add = (Button)sender;
            var pr = (Product)add.Tag;
            showProductPanel.Children.Clear();
            allProductLines = new List<string> { };

            if (p.Name == null||p.Name=="")
            {
                MessageBox.Show("there was no name, try again");
               // p.Name = pr.Name;


                Printproducts(allProducts);

                return;
            }

            else if (p.Description == null||p.Description=="")
            {
                MessageBox.Show("there was no description, try again");

                Printproducts(allProducts);

                return;
            }
            try
            {
                Product newProduct = new Product
                {
                    ImagePath = p.ImagePath,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price
                };
                allProducts.Add(newProduct);
            }
            catch
            {
                MessageBox.Show("the products can't be added.\n" +
                        "some information about the product is incorrect");
            }
            //print allproducts and clears the textboxes
            Printproducts(allProducts);
            nameTextBox.Text = null;
            descriptionBox.Text = null;
            pricebox.Text = null;

            //create string to save in csv fil
            allProductLines = ProductToLines(allProducts);

            File.WriteAllLinesAsync(SavedProductPath, allProductLines);
        }
        //takes the saved inputs from text box and create a new Voucher and saves it in allVoucher list
        private void AddNewVoucherButton_Click(object sender, RoutedEventArgs e)
        {
            Button add = (Button)sender;
            var voucher = (Voucher)add.Tag;
            showVoucherPanel.Children.Clear();
            allVoucherLines = new List<string> { };

            if (v.Code == null || v.Code == "")
            {
                MessageBox.Show("incorrect code, try again");

                PrintVouchers(allVouchers);

                return;
            }

            else if (v.Discount>100||v.Discount<0)
            {
                MessageBox.Show("the discount is too high or too low, try again");

                Printproducts(allProducts);

                return;
            }
            
            
                try
                {
                    Voucher newVoucher = new Voucher
                    {
                        Code = v.Code,
                        Discount = v.Discount
                    };
                    allVouchers.Add(newVoucher);

                }
                catch
                {
                    MessageBox.Show("the voucher couldn't be added correctly.\n" +
                            "some information about the voucher is incorrect");

                }
            

            voucherBox.Text = null;
            precentageBox.Text = null;


            //print allproducts and clears the textboxes
            PrintVouchers(allVouchers);
            
            //create string to save in csv fil
            allVoucherLines = VoucherToLines(allVouchers);

            File.WriteAllLinesAsync(SavedVoucherPath, allVoucherLines);
        }

        //saves  the input from textbox and trys to converts to int
        private void NewPrecentageBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox precent = (TextBox)sender;
            Voucher v = (Voucher)precent.Tag;
            try
            {
                v.Discount = int.Parse(precent.Text);
            }
            catch
            {
                precent.Text = null;
            }
        }
        //saves  the input from textbox
        private void NewCodeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox code = (TextBox)sender;
            //Voucher vo = (Voucher)code.Tag;

            try
            {
                v.Code = code.Text.ToLower();
            }
            catch
            {
            }
        }
        //copying an selected image 
        private void AddImageButton_Click(object sender, RoutedEventArgs e)
        {
            Button addImage = (Button)sender;
            Product p = (Product)addImage.Tag;
            string fileToCopy = "";
            string pathToImage = "";
            string filename = "";
            try
            {//open the document window to select a picture, copy it to temp
                var test = new Microsoft.Win32.OpenFileDialog();
                var testa = test.ShowDialog();
                fileToCopy = test.FileName;
                filename = test.SafeFileName;
                pathToImage = SavedImagesPath + filename;
                File.Copy(fileToCopy, pathToImage);
            }
            catch
            {
            }
            //product gets its image path
            p.ImagePath = filename;

            Button addNewProductButton = new Button
            {
                Content = "add product",
                Tag = p,
                Margin = new Thickness(5),
                Padding = new Thickness(5)
            };
            createProduct.Children.Add(addNewProductButton);
            addNewProductButton.Click += AddNewProductButton_Click;
            Grid.SetColumnSpan(addNewProductButton, 2);
            Grid.SetRow(addNewProductButton, 5);
        }
        private void NewDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox descriptionBox = (TextBox)sender;
            //Product p = (Product)descriptionBox.Tag;
            p.Description = descriptionBox.Text;
        }
        private void NewName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox namebox = (TextBox)sender;
           //Product p = (Product)namebox.Tag;
            p.Name = namebox.Text;
        }

        //saves  the input from textbox and trys to converts to int, if its incorrect clear textbox text 
        private void NewPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox pricebox = (TextBox)sender;
           // Product p = (Product)pricebox.Tag;
            try
            {
                p.Price = double.Parse(pricebox.Text);
            }
            catch
            {
                pricebox.Text = null;
            }
        }
        //turns the product information into strings in a list that can be saved in csv file
        public static List<string> ProductToLines(List<Product> currentProducts)
        {
            List<string> productsLines = new List<string> { };

            foreach (Product p in currentProducts)
            {
                string line = $"{p.ImagePath},{p.Name},{p.Description},{p.Price}";
                productsLines.Add(line);
            }

            return productsLines;
        }

        //turns the voucher information into strings in a list that can be saved in csv file
        public static List<string> VoucherToLines(List<Voucher> currentVouchers)
        {
            List<string> voucherLines = new List<string> { };

            foreach (Voucher v in currentVouchers)
            {
                string line = $"{v.Code},{v.Discount}";
                voucherLines.Add(line);
            }
            return voucherLines;
        }
        //reading products from a csv file returns them in a list
        public static List<Product> ReadProducts(string productFilePath)
        {
            List<Product> products = new List<Product>();

            string[] readText = File.ReadAllLines(productFilePath);
            try
            {
                foreach (string line in readText)
                {
                    //split the line on commas
                    string[] parts = line.Split(',');

                    //create a product with its differant values
                    Product p = new Product
                    {
                        ImagePath = parts[0],
                        Name = parts[1],
                        Description = parts[2],
                        Price = double.Parse(parts[3])
                    };
                    products.Add(p);
                }
            }
            catch
            {
                MessageBox.Show("all products can't be added.\n" +
                    "some information about the product is incorrect");
            }
            return products;
        }

        //reading voucher from a csv file returns them in a list
        public static List<Voucher> ReadVouchers(string voucherFilePath)
        {
            List<Voucher> vouchers = new List<Voucher>();

            string[] readText = File.ReadAllLines(voucherFilePath);
            try
            {
                foreach (string line in readText)
                {
                    //split the line on commas
                    string[] parts = line.Split(',');

                    //create a voucher with its differant values
                    Voucher p = new Voucher
                    {
                        Code = parts[0],
                        Discount = int.Parse(parts[1])
                    };
                    vouchers.Add(p);
                }
            }
            catch
            {
                MessageBox.Show("couldent add voucher correctly");
            }
            return vouchers;
        }
        //printing the products from a list,
        //creates a layout for each product
        private void Printproducts(List<Product> list)
        {
            Label headlabelproducts = new Label 
            {
                Content="All products: ",
                FontSize = 15,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };
            showProductPanel.Children.Add(headlabelproducts);

            foreach (Product product in list)
            {
                newProductGrid = new Grid();
                newProductGrid.ColumnDefinitions.Add(new ColumnDefinition());
                newProductGrid.ColumnDefinitions.Add(new ColumnDefinition ());
                newProductGrid.RowDefinitions.Add(new RowDefinition ());
                newProductGrid.RowDefinitions.Add(new RowDefinition());
                newProductGrid.RowDefinitions.Add(new RowDefinition());
                newProductGrid.RowDefinitions.Add(new RowDefinition());
                newProductGrid.RowDefinitions.Add(new RowDefinition());
                showProductPanel.Children.Add(newProductGrid);
                newProductGrid.Margin = new Thickness(10);
                
                try
                {
                    ImageSource source = new BitmapImage(new Uri(SavedImagesPath + product.ImagePath, UriKind.RelativeOrAbsolute));
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

                    newProductGrid.Children.Add(image);
                    Grid.SetRow(image, 0);
                    Grid.SetColumn(image, 0);

                    Button changeImageButton = new Button
                    {
                        Content="Change image",
                        Margin=new Thickness(10),
                        Tag=product
   
                    };
                    newProductGrid.Children.Add(changeImageButton);
                    Grid.SetRow(changeImageButton, 0);
                    Grid.SetColumn(changeImageButton, 1);
                    changeImageButton.Click += ChangeImageButton_Click;
             

                Label nameLabel = new Label
                { 
                    Content="Name: "
                };
                newProductGrid.Children.Add(nameLabel);
                Grid.SetRow(nameLabel, 1);
                Grid.SetColumn(nameLabel, 0);

                nameProduct = new TextBox
                {
                    Text=product.Name,
                    Tag = product
                };
                newProductGrid.Children.Add(nameProduct);
                Grid.SetRow(nameProduct, 1);
                Grid.SetColumn(nameProduct, 1);
                nameProduct.TextChanged += ChangeName_TextChanged;

                Label descriptionLabel = new Label
                {
                    Content = "Description:  "
                };
                newProductGrid.Children.Add(descriptionLabel);
                Grid.SetRow(descriptionLabel, 2);
                Grid.SetColumn(descriptionLabel, 0);

                descriptionProduct = new TextBox
                {
                    Text = product.Description,
                    Tag = product

                };
                newProductGrid.Children.Add(descriptionProduct);
                Grid.SetRow(descriptionProduct, 2);
                Grid.SetColumn(descriptionProduct, 1);
                descriptionProduct.TextChanged += ChangeDescription_TextChanged1;

                Label priceLabel = new Label
                {
                    Content = "Price:  "
                };
                newProductGrid.Children.Add(priceLabel);
                Grid.SetRow(priceLabel, 3);
                Grid.SetColumn(priceLabel, 0);

                priceProduct = new TextBox
                {
                    Text = $"{product.Price}",
                    Tag = product

                };
                newProductGrid.Children.Add(priceProduct);
                Grid.SetRow(priceProduct, 3);
                Grid.SetColumn(priceProduct, 1);
                priceProduct.TextChanged += ChangePrice_TextChanged;
                

                Button removebutton = new Button
                {
                    Content = "remove product",
                    Tag = product

                };
                newProductGrid.Children.Add(removebutton);
                Grid.SetRow(removebutton, 4);
                removebutton.Click += RemoveProduct_Click;

                Button saveChanges = new Button
                {
                    Content="Save changes",
                    Tag=product
                };
                newProductGrid.Children.Add(saveChanges);
                Grid.SetRow(saveChanges, 4);
                Grid.SetColumn(saveChanges, 1);
                saveChanges.Click += SaveProductChanges_Click;
                }
                catch
                {
                    MessageBox.Show("couldent add product correctly");
                }
            }
        }

        private void ChangeImageButton_Click(object sender, RoutedEventArgs e)
        {
            Button changeImage = (Button)sender;
            Product p = (Product)changeImage.Tag;
            string fileToCopy = "";
            string pathToImage = "";
            string filename = "";
            try
            {
                //open the document window to select a picture, copy it to temp
                var test = new Microsoft.Win32.OpenFileDialog();
                var testa = test.ShowDialog();
                fileToCopy = test.FileName;
                filename = test.SafeFileName;
                pathToImage = SavedImagesPath + filename;
                File.Copy(fileToCopy, pathToImage);
            }
            catch
            {
            }
            //product gets its image path
            p.ImagePath = filename;

            //remove all products
            showProductPanel.Children.Clear();

            //print all products, now with changed image
            Printproducts(allProducts);

            //create string to save in csv fil
            allProductLines = ProductToLines(allProducts);

            File.WriteAllLinesAsync(SavedProductPath, allProductLines);

        }

        private void ChangePrice_TextChanged(object sender, TextChangedEventArgs e)
      {
            TextBox box = (TextBox)sender;
           // Product prod = (Product)box.Tag;

            try
            {
                p.Price =double.Parse(box.Text);
            }
            catch
            {
                box.Text = null;
            }
           
        }

        private void ChangeDescription_TextChanged1(object sender, TextChangedEventArgs e)
        {
            TextBox box = (TextBox)sender;
           // Product prod = (Product)box.Tag;

            try
            {
                p.Description = box.Text;
            }
            catch
            {
            }
           
        }
        private void ChangeName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox namebox = (TextBox)sender;
            Product prod = (Product)namebox.Tag;
            try
            {
                p.Name = namebox.Text;
            }
            catch
            {
            }

        }

        private void SaveProductChanges_Click(object sender, RoutedEventArgs e)
        {
            Button savebutton = (Button)sender;
            var changedProduct = (Product)savebutton.Tag;


           // int index = allProducts.IndexOf(changedProduct);
            showProductPanel.Children.Clear();

            if (p.Name == "" || p.Name == null)
            {
                if (p.Name == "")
                {
                    MessageBox.Show("Empty name, retains previous name");
                }
                p.Name = changedProduct.Name;
                //p.Description = changedProduct.Description;
                //MessageBox.Show("The product was not changed");
                //changedProduct.Name = "TRY AGAIN";

            }

            if (p.Description == "" || p.Description == null)
            {
                if (p.Description == "")
                {
                    MessageBox.Show("could change to an empty Description");
                }
                p.Description = changedProduct.Description;

                //MessageBox.Show("there was no description\n" +
                //    "The product was not changed");
                //changedProduct.Description = "TRY AGAIN";
            }

            if (p.Price == 0)
            {
                p.Price = changedProduct.Price;
            }

            try
            {
    
                changedProduct.Name = p.Name;
                changedProduct.Description = p.Description;
                changedProduct.Price = p.Price;
                MessageBox.Show("changes complete");


                // MessageBox.Show("Change successful");
            }
            catch
            {
                MessageBox.Show("the products can't be added.\n" +
                        "some information about the product is incorrect");
            }
            //print allproducts and clears the textboxes
            Printproducts(allProducts);

            if (changedProduct.Name != "" && changedProduct.Description != "")
            {
                //create string to save in csv fil
                allProductLines = ProductToLines(allProducts);

                File.WriteAllLinesAsync(SavedProductPath, allProductLines);

            }
            else
            {
                MessageBox.Show("couldnt save the changes");
            }
            p.Name = null;
            p.Description = null;
            p.Price = 0;

        }


        //printing the vouhcers from a list,
        //creates a layout for each product
        private void PrintVouchers(List<Voucher> list)
        {
            Label headlabelVoucher = new Label
            {
                Content="All vouchers: ",
                FontSize = 15,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };
            showVoucherPanel.Children.Add(headlabelVoucher);

            foreach (Voucher voucher in list)
            {
                Grid newVoucherGrid = new Grid 
                {
                    Margin=new Thickness(10) 
                };
                newVoucherGrid.ColumnDefinitions.Add(new ColumnDefinition());
                newVoucherGrid.ColumnDefinitions.Add(new ColumnDefinition());
                newVoucherGrid.RowDefinitions.Add(new RowDefinition());
                newVoucherGrid.RowDefinitions.Add(new RowDefinition());
                newVoucherGrid.RowDefinitions.Add(new RowDefinition());
                showVoucherPanel.Children.Add(newVoucherGrid);
                Label code = new Label
                {
                    Content="Voucher code: "
                };
                newVoucherGrid.Children.Add(code);
                Grid.SetRow(code, 0);
                Grid.SetColumn(code, 0);

                voucherCode = new TextBox
                {
                    Text = voucher.Code,
                    Tag = voucher
                };
                newVoucherGrid.Children.Add(voucherCode);
                Grid.SetRow(voucherCode, 0);
                Grid.SetColumn(voucherCode, 1);
                voucherCode.TextChanged += ChangeCode_TextChanged;

                Label precent = new Label
                {
                    Content = "Precent discount: "
                };
                newVoucherGrid.Children.Add(precent);
                Grid.SetRow(precent, 1);
                Grid.SetColumn(precent, 0);

                voucherPrecent = new TextBox
                { 
                    Text= $"{voucher.Discount}",
                    Tag=voucher
                };
                newVoucherGrid.Children.Add(voucherPrecent);
                Grid.SetRow(voucherPrecent, 1);
                Grid.SetColumn(voucherPrecent, 1);
                voucherPrecent.TextChanged += ChangePrecent_TextChanged;

                Button removeV = new Button
                {
                    Content = "remove voucher",
                    Tag = voucher
                };
                newVoucherGrid.Children.Add(removeV);
                Grid.SetRow(removeV, 2);
                Grid.SetColumn(removeV, 0);

                removeV.Click += RemoveVoucher_Click;

                Button saveVoucher = new Button
                {
                    Content = "change voucher",
                    Tag = voucher
                };
                newVoucherGrid.Children.Add(saveVoucher);
                Grid.SetRow(saveVoucher, 2);
                Grid.SetColumn(saveVoucher, 1);
                saveVoucher.Click += SaveVoucherChanges_Click;
            }
        }
        private void SaveVoucherChanges_Click(object sender, RoutedEventArgs e)
        {
            Button saveNewVoucher = (Button)sender;
            var voucher = (Voucher)saveNewVoucher.Tag;
            showVoucherPanel.Children.Clear();

           
            if (voucherString.Length < 3)
            {
                voucherString = voucher.Code;
            }

            if (v.Discount == 0)
            {
                v.Discount = voucher.Discount;
            }

            try
            {
                voucher.Code = voucherString;
                voucher.Discount = v.Discount;
            }
            catch
            {
            }

            PrintVouchers(allVouchers);

            if (voucher.Code.Length>=3)
            {
                //create string to save in csv fil
                allVoucherLines = VoucherToLines(allVouchers);
                File.WriteAllLinesAsync(SavedVoucherPath, allVoucherLines);
            }
            else
            {
                MessageBox.Show("couldnt save the changes");
            }
            v.Discount = 0;
            voucherString = "";

            //try
            //{
            //        v.Code = voucherCode.Text;
            //        v.Discount = int.Parse(voucherPrecent.Text);
                
            //    MessageBox.Show("Change successful");
            //}
            //catch
            //{
            //    MessageBox.Show("the voucher can't be added.\n" +
            //            "some information about the voucher is incorrect");
            //}
            ////voucherCode.Text = null;
            //voucherPrecent.Text = null;
            //v.Code = "";
            //v.Discount = 0;
           
            //print all vouchers
            //PrintVouchers(allVouchers);
        }

        private void ChangePrecent_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox precent = (TextBox)sender;
            Voucher changevoucher = (Voucher)precent.Tag;
      
            try
            {
                v.Discount = int.Parse(precent.Text);
            }
            catch
            {
               //precent.Text = null;
            }
        }
        private void ChangeCode_TextChanged(object sender, TextChangedEventArgs e)
       {
         TextBox codebox = (TextBox)sender;
            //Voucher vo = (Voucher)codebox.Tag;

            voucherString = codebox.Text;
                
                    //v.Code = codebox.Text;
               
        }

        //removes vocuher from grid and list and re-prints the current list, updating csv file
        private void RemoveVoucher_Click(object sender, RoutedEventArgs e)
        {
            Button remove = (Button)sender;
            Voucher vouch = (Voucher)remove.Tag;
            int indexToDelete = allVouchers.IndexOf(vouch);
            showVoucherPanel.Children.Clear();
            allVouchers.RemoveAt(indexToDelete);

            PrintVouchers(allVouchers);

            allVoucherLines = VoucherToLines(allVouchers);
            File.WriteAllLinesAsync(SavedVoucherPath, allVoucherLines);
        }
        //removes product from grid and list and re-prints the current list, updating csv file
        private void RemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            Button remove = (Button)sender;
            Product prod = (Product)remove.Tag;
            int indexToDelete = allProducts.IndexOf(prod);
            showProductPanel.Children.Clear();
            allProducts.RemoveAt(indexToDelete);

            Printproducts(allProducts);

            allProductLines = ProductToLines(allProducts);
            File.WriteAllLinesAsync(SavedProductPath, allProductLines);
        }
    }
}
