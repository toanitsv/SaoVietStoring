   M S B A M L   `   `   `  ���� g  cMicrosoft.ReportViewer.WinForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3atSclr-namespace:Microsoft.Reporting.WinForms;assembly=Microsoft.Reportviewer.WinFormsMicrosoft.Reporting.WinForms   SaoVietStoring2   ,SaoVietStoring.Views.InputDetailReportWindow   [rpvSclr-namespace:Microsoft.Reporting.WinForms;assembly=Microsoft.Reportviewer.WinForms  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$$  Storing System - Detail Report�� � WindowState$ 	Maximized=�$	��450��$	��300�� � WindowStartupLocation$ CenterScreen=�$��13(�.��� $��5,5,5,5q� � RowDefinitions �� $
��Auto ��� $��* �.+���  �� Orientation$ 
Horizontal=�.+��� $��Production No.:��$��Center=��� -    ��Name$ txtProductNo��$	��150��$��5,0,0,0q��� -    ��Name$ 	btnReport��$��Preview Input��$��10,0,0,0q�$	��100��$
��Bold"� �� 	IsDefault . �� -   $ btnReportOutput��$��Preview Output��$��10,0,0,0q�$	��100��$
��Bold"�&   System.Windows.Controls.DataGrid  -     Name$ dgDetailReport��$��0,5,0,0q�$��1��	   AutoGenerateColumns	 .  
   CanUserAddRows
 .     CanUserResizeRows .   �� AlternationCount$ 2��   AlternatingRowBackground
 �����   
IsReadOnly .    CanUserReorderColumns .     SelectionUnit$ FullRow=�   HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����$��12(�   ColumnHeaderStyle ��  �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"  .���  �� Property � �� �� Value$ Center=���  � ��$ 13(�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Barcode��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *	Barcode+  $ CartonNo�� �� *
CartonNo+  $ SizeNo��   CanUserSort .   �� *SizeNo+  $ Gross
Weight (kg)�� �� *GrossWeight+  $ Actual
Weight (kg)�� �� *ActualWeight+  $ Difference
Percent (%)�� �� *DifferencePercent+  $ Inputed
Time�� �� *InputedTime+  $ Location�� .   �� *
Location+