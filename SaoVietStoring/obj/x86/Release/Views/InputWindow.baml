   M S B A M L   `   `   `  ����   SaoVietStoring&     SaoVietStoring.Views.InputWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$#  Storing System - INPUT CARTON��$��20(�$	��700��$
��1024�� � WindowState$ 	Maximized=� � WindowStartupLocation$ CenterScreen=� � 	Resources    RoundCorner �� 
TargetType �� Property �� Value �� 
TargetType �Name	 ��Name
 �� CornerRadius � Offset � Color �� Triggers P� Property P� Value �� 
TargetName  CompleteBackground �� 
StartPoint �� EndPoint  IncompleteBackground��%�  &	        &	 �    &	 -    �� " ��.���  � �� �� " ��.R�� $
 grid��.+��� $	 border��$
 0,0,0,0��
���   �$��1q���
� .��� $	 0.3Q�$ Bisque��._��� $��Center=�$��Center=� P�  � �� . . ��� $ border�� � �� 
� .��� $ 	LightBlue��$	 0.3Q�P�  �@J�IsMouseOver . . ��� $ border�� � �� �#F00F�� $ border�� � ��$ 2q��� $ border�� � �� 
� .��� $ LightSeaGreen��$	 0.3Q��� $	 0,0&�$	 0,1&���� $	 0.5Q�$ 
LightGreen��� $	 0.8Q�$ 
LightGreen���� $	 0,0&�$	 0,1&���� $	 0.3Q�$ Tomato��� $	 0.7Q�$ Tomato��.��� -   $ 	gridInput��$��5q� � RowDefinitions �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $��* �.+�� $��1��$��50�� � ColumnDefinitions �� $��* ��� $��10 ��� $��* �.+�� $��0��.+��� -    ��Name$ btnComplete��#���   $��Complete Carton��$��20(�$��Caribi)�� $��2��.+��� -   $ btnIncomplete��#���   $��Incomplete Carton��$��20(�$��Caribi)�� $��2��$��0,0,0,0q� �� �� $��10 ��� .+��� $��0�� �� Orientation$ 
Horizontal=�$
��Left=�$��0,0,0,5q�.+��� -    ��Name$ txtMinActualWeight��$��60�� �� TextAlignment$ Center=�$��30(�$��1��$��Bottom=�
���   ��� 
����  �$!�� ≤ Actual Weight(kg) ≤ ��$��Bottom=��� -   $ txtMaxActualWeight��$��60��$ Center=�$��30(�$��20��$��Bottom=�
���   ��� -   $	 brTime��$��2��._�(�  (� Stretch$ Uniform=�$��Stretch=�$��Stretch=�.���� -    ��Name$ tblStatusTime��$��70(� �� TextAlignment$ Center=�$��Caribi)�$��Center=�$
��Wrap=��� -	   $	 
brComplete��$��4��
���   �$y�	Collapsed=�#��[ $��1,1,1,1q�._�� $��5q� �� $
��auto ��� $��* �.+�� $��0�� �� �� $��10 ��� .+�� $��0��$	��Top=� �� �� .+�� $��1��$
��auto�� �� $	��200 ��� $��* �.+��� $��0��$��1. Barcode: ��$��Center=��� $��1��$ Vertical=�.+��� -
   $ txtBarCodeComplete��
���   �{�.  �� -   $ popupPosition��$��Popup Position��$��0��$��0,5,0,0q�� -    �Name$ popInputSubPO��$��Scroll=� � AllowsTransparency .  � PlacementTarget ��  �� ElementName$ popupPosition��.���� 
���   �$��1,1,1,1q�  �� Padding$  5q�
�������._��� $ Vertical=�.+��� $ 
Horizontal=�.+��� $
��PO#:��$
��Bold"�$��20(�$��Bottom=�$	��100���� -   $ txtProductNo��{�.  $��30(�$
��Bold"�$	��120��
���   �! � ToolTip$! Bấm Phím Enter���� -   $ btnProductNo��$��0��{�.  �� $ 
Horizontal=�$��0,5,0,0q�.+��� $��	CartonNo:��$
��Bold"�$��20(�$��Bottom=�$	��100���� -   $ txtCartonNo��{�.  $��30(�$
��Bold"�$	��100��
���   �$! Bấm Phím Enter���� -   $ btnCartonNo��$��0��{�.  �� -   $ btnBarcodeComplete��$��Right=�$��0��$��2��
��� � �� -   $ gridCartonDetail��$��2��$��Right=�.+��� $ Vertical=�$	��Top=�.+��� -   $ tblProductNo��$��80(�$
��Bold"�$
��Left=��� -   $! tblSizeItemQuantityCartonNo��$��50(�$
��Left=�� $��1��$��0,0,0,0q�$��Bottom=� �� $��* ��� $��40 ��� $��* ��� $��40 ��� $��* � �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��0��$��4��$��3. Gross Weight(kg)���� $��2��$��4��$��4. Actual Weight(kg)��
����  ��� $��2��$��2��$��6��$��0,0,5,0q�$��6. Difference Percent(%)��$��Center=�$��Right=��� $��6��$��4��$��5. Result(Hi,Pass,Low)���� $��0��$��5��$��0,5,0,0q�
���   �$��1,1,1,1q�._��� -   $ tblGrossWeight��$��0��$
��Bold"�$	��130(�$ Center=�
" � Tag$" 0���� $��2��$��5��$��0,5,0,0q�
����  �$��1,1,1,1q�._��� -   $ tblActualWeight��$��0��$
��Bold"�$	��130(�$ Center=�
����  �$" 0���� $��4��$��6��$��0,0,0,0q�
���   �$��1,0,1,1q�._��� -   $ tblDifferencePercent��$��0��$
��Bold"�$��35(�$ Center=��� -   $	 brResult��$��6��$��5��$��0,5,0,0q�
���   �$��1,1,1,1q�._��� -   $ 	tblResult��$	��...��$
��Bold"�$��80(�$ Center=�$��Bottom=�$
��Wrap=�