   M S B A M L   `   `   `  ����g  cMicrosoft.ReportViewer.WinForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3atSclr-namespace:Microsoft.Reporting.WinForms;assembly=Microsoft.Reportviewer.WinFormsMicrosoft.Reporting.WinForms  5   	    SaoVietStoring2   ,SaoVietStoring.Views.InputDetailReportWindow   [rpvSclr-namespace:Microsoft.Reporting.WinForms;assembly=Microsoft.Reportviewer.WinForms  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$$  Storing System - Detail Report��5   	    � WindowState$ 	Maximized=�60   $	��450��6H   $	��300��6W    � WindowStartupLocation$ CenterScreen=�6g   $��13(�6�   .��5      � $��5,5,5,5q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      $��* �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $��Production No.:��6   $��Center=�6/   �� 5      -   5         ��Name$ txtProductNo��5      $	��150��6,   $��5,0,0,0q�6;   6L   �� 5      -   5         ��Name$ 	btnReport��5      $��Preview Input��6(   $��10,0,0,0q�6@   $	��100��6R   $
��Bold"�6a    �� 	IsDefault . 6�   �� 5      -   5        $ btnReportOutput��5      $��Preview Output��6.   $��10,0,0,0q�6G   $	��100��6Y   $
��Bold"�6h   6z   5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dgDetailReport��5   $   $��0,5,0,0q�6   $��1��6<   	   AutoGenerateColumns	 .  
   CanUserAddRows
 .     CanUserResizeRows .   �� AlternationCount$ 2��5   D      AlternatingRowBackground
 �����   
IsReadOnly .    CanUserReorderColumns .     SelectionUnit$ FullRow=�5   1      HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����$��12(�5   [      ColumnHeaderStyle 5      �� 5       �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�5      ��  �� Property � �� �� Value$ Center=�6C   �� 5       � ��$ 13(�61   5         Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Barcode��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 66   �� *	Barcode+  5      $ CartonNo��6%    67   �� *
CartonNo+  5      $ SizeNo��6%      CanUserSort .   65   �� *SizeNo+6P     5       $ Gross
Weight (kg)��6%    6D   �� *GrossWeight+  5!      $ Actual
Weight (kg)��6%    6E   �� *ActualWeight+  5"      $ Difference
Percent (%)��6%    6I   �� *DifferencePercent+  5#      $ Inputted
Time��6%    6@   �� *InputtedTime+  5$      $ Location��6%    .   67   �� *
Location+6T   5&      5*      5+      