   M S B A M L   `   `   `  ����   SaoVietStoring.    (SaoVietStoring.Views.ReWeighCartonWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$%  Storing System - ReWeigh Carton��$	��400��$	��700�� � 
ResizeMode$ NoResize=� � SizeToContent$ WidthAndHeight=� � WindowStartupLocation$ CenterScreen=�$��18(�.��� $��5q� � RowDefinitions �� $
��auto ��� $��* �.+�� $��0�� � ColumnDefinitions �� $
��auto ��� $��* �.+���  �� Orientation$ 
Horizontal=�$��0��.+��� $��	Barcode: ��$��Bottom=�$	��100���� -    ��Name$ 
txtBarcode��$	��250��
���   ��� -    ��Name$ 
btnBarcode��$��0��{�. 	 �� 	IsDefault	 . �� $��1��._��� -   
 ��Name$
 
stkConfirm��$��Right=�$ 
Horizontal=�$y�	Collapsed=�.+��� $��Security Code:��$��Bottom=�
���  ��G� -    G�Name$ txtPassword��$	��120��
���  ��$��5,0,0,0q�$��2,2,2,2q��� -   $ btnSave��$
��Save��{�.  $��5,0,0,0q�$
��Bold"�$��60��� $��1��$��
100,10,0,0q� �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $��* �.+��� -    ��Name$ txtProductNo���� -   $ 	txtSizeNo��$��1��$��0,5,0,0q��� -	   $ txtCartonNo��$��2��$��0,5,0,0q�� $��3��$��0,10,0,0q� �� �� $��10 ��� .+��� $ Vertical=�$��0��.+��� $��Old Weight (kg)��$��Center=�$��0,0,0,5q��� 
���   �$��1,1,1,1q�._��� -
   $ txtOldWeight��$��0��$	��100(�$
��Bold"�$��Center=��� $ Vertical=�$��2��.+��� $��New Weight (kg)��$��Center=�
����  �$��0,0,0,5q��� 
����  �$��1,1,1,1q�._��� -   $ txtNewWeight��$��0��$	��100(�$
��Bold"�
����  �$��Center=��� -   $ 	btnWeight��$��4��$��0,10,0,0q�$��[Weigh]��$��50��
��� � �$��Bottom=�