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
using Kait.ViewModel;
using Kait.ViewModel.Primitive;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Kait.View.Pages
{
    /// <summary>
    /// Interaction logic for NewInvoice.xaml
    /// </summary>
    public partial class NewInvoice : Page
    {
        private NewInvoiceViewModel VM = new NewInvoiceViewModel(DialogCoordinator.Instance);
        
        public NewInvoice()
        {
            InitializeComponent();
            DataContext = VM;

        }

        public NewInvoice(InvoiceViewModel invoice)
        {
            InitializeComponent();
            DataContext = new NewInvoiceViewModel(DialogCoordinator.Instance,invoice);

        }

        //
        //Control members
        //

        private void NavigateToTab(TabType tabType)
        {
            InvoiceNavTab.SelectedIndex = (int)tabType;
        }
        private enum TabType
        {
            ProductDetails,//0
            ClientDetails,//1
            PrintView//2
        }

        //
        //Interface(UI) members
        //

        //Backbutton at the top
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.PageHostService.CanGoBack)
            {
                MainWindow.PageHostService.GoBack();
            }


        }

        //TODO: Convert CodeBehind Code to MVVM model

        // The following non standard code is just a temporary work around for 
        // Adding Purchase items to print view


        private void NavigateToPrintPreview(object sender, RoutedEventArgs e)
        {
            (DataContext as NewInvoiceViewModel).SaveInvoiceCmd.Execute(null);
            GenerateItemRowsToPrintView();
            //temporary turn around for developing print preview
            
            Console.WriteLine("Column count with print preview table {0}",PrintDocumentProductTable.Columns.Count);
            //PrintDocumentProductTable.Columns.ElementAt(1).Width = new GridLength(300);
            PrintInvoice();
        }



        void GenerateItemRowsToPrintView() {
            NewInvoiceViewModel viewModel = (NewInvoiceViewModel)DataContext;

            TableRowGroup ItemTableRowGroup = new TableRowGroup();
            ItemTableRowGroup.Rows.Add(BuildHeaderRow());

            /*
            TableRow underlineBelow = new TableRow();
            TableCell cell = new TableCell()
            {
                ColumnSpan = 9,
                Padding = new Thickness(0),

            };
            cell.Blocks.Add(
              new BlockUIContainer(new Line()
              {
                  X1 = 0,
                  X2 = 1200,
                  Y1 = 0,
                  Y2 = 1,
                  Stroke = new SolidColorBrush(Color.FromRgb(200, 200, 200)),
                  StrokeThickness=1
              })
            );


            underlineBelow.Cells.Add(cell);

            ItemTableRowGroup.Rows.Add(underlineBelow);
            */
            bool alternateColor = false;
            foreach (InvoiceProductsViewModel item in viewModel.AddedInvoiceProducts.ToList())
            {
                var itemRow = BuildItemRow(item);
                itemRow.Background = new SolidColorBrush(alternateColor?Color.FromRgb(246,246,246):Color.FromRgb(253,253,253));
                alternateColor = !alternateColor;
                ItemTableRowGroup.Rows.Add(itemRow);
                TableRow underlineBelow = new TableRow();
                TableCell cell = new TableCell()
                {
                    ColumnSpan = 9,
                    Padding = new Thickness(0),

                };
                cell.Blocks.Add(
                  new BlockUIContainer(
                      new Line()
                      {
                          X1 = 0,
                          X2 = 1200,
                          Y1 = 0,
                          Y2 = 1,
                          Stroke = new SolidColorBrush(Color.FromRgb(220, 220, 220)),
                          StrokeThickness = 1
                      }
                  )
                  {
                      Padding=new Thickness(0.10)
                  }
                );

                ItemTableRowGroup.Rows.Add(underlineBelow);


            }

            for (int i = 0; i < 12 - viewModel.AddedInvoiceProducts.Count; i++)
            {
                ItemTableRowGroup.Rows.Add(CreateEmptyRow());
            }

            TableRow footerRow = new TableRow()
            {
                Background = new SolidColorBrush(Color.FromRgb(230, 230, 230))
            };

            TableCell totalText = CreateTableCellWithText("TOTAL");
            totalText.ColumnSpan = 2;
            totalText.TextAlignment = System.Windows.TextAlignment.Left;
            totalText.Padding = new Thickness(10);

            footerRow.Cells.Add(totalText);
            footerRow.Cells.Add(CreateTableCellWithText("",ApplyFooterCellStyle));
            footerRow.Cells.Add(CreateTableCellWithText("", ApplyFooterCellStyle));
            footerRow.Cells.Add(CreateTableCellWithText("", ApplyFooterCellStyle));
            footerRow.Cells.Add(CreateTableCellWithText("", ApplyFooterCellStyle));
            footerRow.Cells.Add(CreateTableCellWithText((viewModel.NewInvoice.TotalTax / 2).ToString(), ApplyFooterCellStyle,true));
            footerRow.Cells.Add(CreateTableCellWithText((viewModel.NewInvoice.TotalTax / 2).ToString(), ApplyFooterCellStyle,true));
            footerRow.Cells.Add(CreateTableCellWithText(viewModel.NewInvoice.Total.ToString(), ApplyFooterCellStyle,true));
            ItemTableRowGroup.Rows.Add(footerRow);
            PrintDocumentProductTable.RowGroups.Clear();
            PrintDocumentProductTable.RowGroups.Add(ItemTableRowGroup);

        }

        SolidColorBrush HeaderForegroundColor = new SolidColorBrush(Color.FromRgb(50, 50, 50));
        void ApplyHeaderStyle(TableCell tableCell)
        {

            tableCell.TextAlignment = System.Windows.TextAlignment.Center;
            tableCell.Padding = new Thickness(0, 5, 0, 5);
            tableCell.Foreground = HeaderForegroundColor;
            tableCell.BorderThickness = new Thickness(0, 0, 0, 1);
            tableCell.BorderBrush = new SolidColorBrush(Color.FromRgb(100, 100, 100));
            tableCell.FontWeight = FontWeights.Bold;

        }
        void ApplyItemCellStyle(TableCell tableCell)
        {

            tableCell.TextAlignment = System.Windows.TextAlignment.Center;
            tableCell.Padding = new Thickness(0,10,0,10);
            tableCell.Foreground = new SolidColorBrush(Color.FromRgb(50, 50, 50));
            tableCell.FontWeight = FontWeights.Normal;
            tableCell.FontSize = 14;

        }
        void ApplyFooterCellStyle(TableCell tableCell)
        {

            tableCell.TextAlignment = System.Windows.TextAlignment.Center;
            tableCell.Padding = new Thickness(10);
            tableCell.FontWeight = FontWeights.Bold;

        }
        TableRow BuildHeaderRow()
        {
            TableRow headerRow = new TableRow();
            headerRow.Cells.Add(CreateTableCellWithText("SL NO",ApplyHeaderStyle));
            headerRow.Cells.Add(CreateTableCellWithText("PRODUCT", ApplyHeaderStyle));
            headerRow.Cells.Add(CreateTableCellWithText("HSN", ApplyHeaderStyle));
            headerRow.Cells.Add(CreateTableCellWithText("QTY", ApplyHeaderStyle));
            headerRow.Cells.Add(CreateTableCellWithText("UNIT PRICE", ApplyHeaderStyle));
            headerRow.Cells.Add(CreateTableCellWithText("GST RATE", ApplyHeaderStyle));
            headerRow.Cells.Add(CreateTableCellWithText("CGST", ApplyHeaderStyle));
            headerRow.Cells.Add(CreateTableCellWithText("SGST", ApplyHeaderStyle));
            headerRow.Cells.Add(CreateTableCellWithText("AMOUNT", ApplyHeaderStyle));
            headerRow.Background = new SolidColorBrush(Color.FromRgb(230, 230, 230));
            return headerRow;
        }

        TableRow BuildItemRow(InvoiceProductsViewModel item)
        {
            TableRow itemRow = new TableRow();

            itemRow.Cells.Add(CreateTableCellWithText(item.SlNo.ToString(), ApplyItemCellStyle));
            itemRow.Cells.Add(CreateTableCellWithText(item.Name, ApplyItemCellStyle));
            itemRow.Cells.Add(CreateTableCellWithText(item.HSN, ApplyItemCellStyle));
            itemRow.Cells.Add(CreateTableCellWithText(item.Quantity.ToString(), ApplyItemCellStyle));
            itemRow.Cells.Add(CreateTableCellWithText(item.Price.ToString(), ApplyItemCellStyle, true));
            itemRow.Cells.Add(CreateTableCellWithText(item.Tax.Rate.ToString() + "%", ApplyItemCellStyle,false));
            itemRow.Cells.Add(CreateTableCellWithText(Decimal.Round(item.TotalTax/2, Convert.ToInt16(App.GetConfig("RoundOffValues"))).ToString(), ApplyItemCellStyle,true));
            itemRow.Cells.Add(CreateTableCellWithText(Decimal.Round(item.TotalTax / 2, Convert.ToInt16(App.GetConfig("RoundOffValues"))).ToString(), ApplyItemCellStyle,true));
            itemRow.Cells.Add(CreateTableCellWithText(item.Total.ToString(), ApplyItemCellStyle,true));
            return itemRow;

        }
        
        TableRow CreateEmptyRow()
        {
            TableRow tableRow = new TableRow();
            for (int i = 0; i < 9/*TODO:Remove hardcoded values*/; i++)
            {
                tableRow.Cells.Add(CreateTableCellWithText("", ApplyItemCellStyle));
            }
            return tableRow; 
        }
        
        TableCell CreateTableCellWithText(String text, Action<TableCell> ApplyStyles=null , bool IsCurrency=false) {
            TableCell tableCell = new TableCell();


            TextBlock textBlock = new TextBlock
            {
                Text = (IsCurrency? "₹" : "")+text,
                TextWrapping=TextWrapping.Wrap
            };

            Block cellBlock = new BlockUIContainer(textBlock);
            ApplyStyles?.Invoke(tableCell);
            tableCell.Blocks.Add(cellBlock);
            return tableCell;
        }

        private void PrintInvoice()
        {
            PrintDialog printDialog = new PrintDialog();
            IDocumentPaginatorSource source = documentReader.Document;
            DocumentPaginator paginator = source.DocumentPaginator;
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintDocument(paginator, "Printing Invoice");
            }
        }
    }
}
