   M S B A M L   `   `   `  ����   SaoVietStoring-    'SaoVietStoring.Views.ImportOrdersWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$  Import Orders��$	��600��$
��1000��.���  � RowDefinitions �� $��* ��� $
��auto ��� $
��auto �.+�&   System.Windows.Controls.DataGrid  -     Name$ dgOrders��   AutoGenerateColumns .     
IsReadOnly .    AlternatingRowBackground
 ����� �� AlternationCount$ 2��   HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����	 � 	Resources	 *  $System.Windows.Controls.DataGridCell
 �� 
TargetType �� Triggers P� Property P� Value �� Property �� Value��%j   '        �� "
   P�  �@ 
IsSelected . . ���  � ��
 ��iA���  � ��
 �����   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto    Header$ Pro. No��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *	ProductNo+  $
 Auto $ 
Outer Code�� �� *	OuterCode+  $
 Auto $ 
Article No�� �� *	ArticleNo+  $
 Auto $ 	Shoe Name�� �� *
ShoeName+  $
 Auto $ Delivery Country�� �� *DeliveryCountry+  $
 Auto $ Quantity�� �� *
Quantity+  $
 Auto $
 Size�� �� *Size+  $
 Auto $ CSD Date�� �� *	CSDDate+�� -    ��Name$ 	btnImport��$��1��$��0,5,0,0q�$	��150��$��Right=�$��Import�� � ToolTip$ Click to Import Orders��$
��Bold"�{�.  