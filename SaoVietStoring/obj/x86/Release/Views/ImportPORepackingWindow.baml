   M S B A M L   `   `   `  ����   SaoVietStoring2    ,SaoVietStoring.Views.ImportPORepackingWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$*  $Storing System - Import PO Repacking��$	��300��$	��500�� � WindowStartupLocation$ CenterScreen=�.��� $��5q� � RowDefinitions �� $
��auto ��� $
��auto ��� �� $
��auto �.+���  �� Orientation$ 
Horizontal=�.+��� $��Product No: ��$��14(�$��Bottom=��� -    ��Name$ txtPORepacking��$��14(�$	��150��$��5,0,5,0q��� -    ��Name$ btnAddPORepacking��$��14(�$	��Add��$��70���� $��1��$��PO Repacking List��$��0,10,0,2q�$��14(�$
��Bold"�&   System.Windows.Controls.DataGrid  -     Name$ dgPORepacking��$��2��   AutoGenerateColumns .     
IsReadOnly . 	   AlternatingRowBackground
	 �����
 �� AlternationCount$
 2��   HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 ����� � 	Resources *  $System.Windows.Controls.DataGridCell �� 
TargetType �� Triggers P� Property P� Value �� Property �� Value��%j   '        �� "   P�  �@ 
IsSelected . . ���  � ��
 ��iA���  � ��
 �����   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto    Header$	 PO#��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *	ProductNo+  $
 Auto $ Created Date�� �� *CreatedTime+ �� StringFormat$ 
MM/dd/yyyy��4  .System.Windows.Controls.DataGridTemplateColumn     CellTemplate �� .R��� $ btnClear��-   $��X��$��20��$��20��
����  �$��11(��� $��3��$ 
Horizontal=�$��0,5,0,0q�.+��� -   $ btnSave��$��14(�$
��Save��$��28��$	��Top=�$��70���� -    ��Name$ stkControlAccount��$ 
Horizontal=�$��10,0,0,0q�$y�	Collapsed=�.+��� $��Security Code: ��$��Bottom=�$��14(�
���  ��G� -    G�Name$ txtPassword��$	��120��
���  ��$��2,2,2,2q�$��5,0,0,0q�$��16(��� -	   $ 
btnConfirm��$��Confirm��$
��Bold"�$��13(�$��70��$��5,0,0,0q�