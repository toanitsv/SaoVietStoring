   M S B A M L   `   `   `  ����   SaoVietStoring,    &SaoVietStoring.Views.CheckIssuesWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$$  Storing System - Report Issues�� � SizeToContent$ WidthAndHeight=�$	��400��$	��500�� � WindowStartupLocation$ CenterScreen=� � WindowStyle$ 
ToolWindow=� � 	Resources    LinearBackground �� 
StartPoint �� EndPoint � Offset � Color��%b   &	        �� $	 0,0&�$	 0,1&���� $	 0.3Q�$ Silver��� $ 1Q�$ Transparent��.��� #��[  	 � RowDefinitions	 �� $
��Auto ��� $
��Auto ��� $��* ��� $
��Auto �.+�� $��0��
 � ColumnDefinitions
 �� $��* ��� $��2* ��� $	��100 �.+��� $��Security Code :��$��0��$��18(�$��Center=�G� -    G�Name$ txtSecurityCode��$��1��$��20(�
���   �$	��300��$��5,0,5,0q��� -    ��Name$ 
btnAccecpt��$��2��$��Accept��$��16(�$
��Bold"� �� 	IsDefault . � $��1��$��0,10,0,0q�.+��� -    ��Name$ 	tblStatus��$��18(�
����  �� $��2��
 �� �� .+��� -    ��Name$ groupChooseReason��$��0��$��Choose a Reason��$��15(�$y�	Collapsed=�
���   �.��� -    �Name$ 
gridIssues��$��10,0,0,0q�$y�	Collapsed=�$	��200���� -   $ groupUpdateCarton��$��1��$��Update Weight��$��15(�$y�	Collapsed=�$��10,5,0,0q�
���   �.��� 	 �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto �.+��� $��0�� �� Orientation$ 
Horizontal=�.+��� $	��PO:��$	��100��$��Bottom=��� -   $ txtPO��$��Bottom=�
���  ���� $��2��$ 
Horizontal=�$��0,5,0,0q�.+��� $��Barcode:��$	��100���� -	   $ 
txtBarcode��
���  ���� $��3��$ 
Horizontal=�$��0,5,0,0q�.+��� $��	CartonNo:��$	��100��$��Bottom=��� -
    ��Name$ txtCartonNo��$	��100��
���   �
���  ���� $��4��$ 
Horizontal=�$��0,5,0,0q�.+��� $��SizeNo:��$	��100��$��Bottom=��� -   $ 	txtSizeNo��$	��100��
���   �
���  ���� $��6��$ 
Horizontal=�$��0,5,0,0q�.+��� $��Actual Weight:��$	��100��$��Bottom=��� -   $ txtActualWeight��$	��100��
���   �
���  ���� $��7��$ 
Horizontal=�$��0,10,0,0q�.+��� -   $ btnSave��{�.  $
��Save��$��80��� $��3��$��0,5,0,0q�.+��� $��0��$ 
Horizontal=�.+��� -   $ btnUpdateWeight��$��Update Weight��$��18(�
��� � �{�.  