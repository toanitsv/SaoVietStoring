   M S B A M L   `   `   `  ����  SaoVietStoring2    ,SaoVietStoring.Views.ImportPORepackingWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$*  $Storing System - Import PO Repacking��5   	   $	��300��66   $	��500��6C    � WindowStartupLocation$ CenterScreen=�6O   .��5      � $��5q�6    � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5	      �� 5
      $
��auto �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $��Product No: ��6   $��14(�6,   $��Bottom=�6:   �� 5      -   5         ��Name$ txtPORepacking��5      $��14(�6.   $	��150��6<   $��5,0,5,0q�6K   �� 5      -   5         ��Name$ btnAddPORepacking��5      $��14(�60   $	��Add��6>   $��70��6L   6Z   5      �� 5   
   $��1��6   $��PO Repacking List��6!   $��0,10,0,2q�6:   $��14(�6L   $
��Bold"�6Z   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dgPORepacking��5       $��2��6      AutoGenerateColumns .     
IsReadOnly . 	   AlternatingRowBackground
	 �����
 �� AlternationCount$
 2��5   8      HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 ����� � 	Resources 5      *  $System.Windows.Controls.DataGridCell �� 
TargetType �� Triggers P� Property P� Value �� Property �� Value��%�   '        �� 5      "  6    5      P� 5       �@ 
IsSelected . . �5      ��  � ��
 ��iA�6;   �� 5       � ��
 �����6;   5      5      5         Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5       ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6%      Header$	 PO#��62   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6?   �� *	ProductNo+  5!      $
 Auto 6%   $ Created Date��62    6H   �� *CreatedTime+ �� StringFormat$ 
MM/dd/yyyy��4  .System.Windows.Controls.DataGridTemplateColumn  5"         CellTemplate 5#      �� 5$      .R�5%      �� $ btnClear��6%   -   5        $��X��5%   7   $��20��6C   $��20��6Q   
����  �$��11(�6o   6}   5&      5(      5*      �� 5+   
   $��3��6   $ 
Horizontal=�6"   $��0,5,0,0q�6;   .+�5,      �� -   5        $ btnSave��5,      $��14(�6&   $
��Save��64   $��28��6C   $	��Top=�6O   $��70��6g   6u   �� 5-      -   5         ��Name$ stkControlAccount��5-      $ 
Horizontal=�64   $��10,0,0,0q�6M   $y�	Collapsed=�6_   .+�5.      �� $��Security Code: ��6   $��Bottom=�63   $��14(�6N   
���  ��6\   G� 5/      -   5         G�Name$ txtPassword��5/      $	��120��63   
���  ��$��2,2,2,2q�6T   $��5,0,0,0q�6n   $��16(�6   �� 50      -	   5        $ 
btnConfirm��50      $��Confirm��6-   $
��Bold"�6?   $��13(�6Q   $��70��6_   $��5,0,0,0q�6j   6{   51      52      53      54      