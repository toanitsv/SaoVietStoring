   M S B A M L   `   `   `  ����   SaoVietStoring    SaoVietStoring.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$  Storing System��$	��400��$	��700�� � 
ResizeMode$ CanMinimize=� � WindowStyle$ SingleBorderWindow=� � WindowStartupLocation$ CenterScreen=� � 	Resources    RoundCorner �� 
TargetType �� Property �� Value �� 
TargetType	 �Name
 ��Name �� CornerRadius � Offset � Color �� Triggers P� Property P� Value �� 
TargetName  LinearBackground �� 
StartPoint �� EndPoint��%D  &	        &	 �    �� " ��.���  � �� �� " ��.R�� $
	 grid��.+��� $
 border��$ 0,0,0,0��
���   �$��1q���
� .��� $	 0.3Q�$ Bisque��._��� $��Center=�$��Center=� P�  � �� . . ��� $ border�� � �� 
� .��� $ 	LightBlue��$	 0.3Q�P�  �@J�IsMouseOver . . ��� $ border�� � �� �#F00F�� $ border�� � ��$ 2q��� $ border�� � �� 
� .��� $ LightSeaGreen��$	 0.3Q��� $	 0,1&�$	 1,0&���� $
 -0.5Q�$ LightSeaGreen��� $ 1Q�$ Transparent��.���  � RowDefinitions �� $
��auto ��� .+�� $��0�� � ColumnDefinitions �� �� $
��auto �.+�o� $��0��#��[ $��Caribi)�$��14(�$��Center=�$��Center=�.@�m� $��Tools��.@�m� -    m�Name$ miInput��{�.  $��Input��m� -   $ miOutput��{�.  $��Output��m� -   $ miReWeighCarton��{�.  $��Re-Weigh Carton��m� -   $ miImportPORepacking��{�.  $��Import PO Repacking��m� $��Report��.@�m� -   $ miInputReport��$��Inputing Report��m� -   $ miDetailReport��$��Detail Report��m� $
��Help��.@�m� -   $ miAbout��$��About me��m� -	   $ 	miVersion��$��Version���� -
    ��Name$ 
btnReLogin��$��1��$��Login��{�.  $y�	Collapsed=�
����� �� $��1��.+��� -    ��Name$ imgBackground��$��Images/main.jpg��$y�	Collapsed=�� -   $	 	gridLogin��$��1��#��[  �� �� $
��auto ���  �� �� $
��auto ��� .+��� -   $
 brLogin��$��1��$��1��$	��380��._��  �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto �.+�� -   $ 	 backgroundStatusConnection��$��0��$��30��
��� � �.+��� -    ��Name$ tblConnectionStatus��$��Center=�$%��--- Login to Storing System ---��
���   �$��Center=�$��15(�$
��Bold"�� $��1��$��30��$��0,5,0,0q� �� $��* ��� $��2* �.+�� $��0��
�������$��0,0,5,0q�.+��� $��
User Name:��$��Center=�$��Center=�$��14(�� $��1��.+��� -    ��Name$ txtUserName��$��Center=�
���   �$��30��$��14(�$��Center=�{�.  � $��2��$��30��$��0,5,0,0q� �� $��* ��� $��2* �.+�� $��0��
�������$��0,0,5,0q�.+��� $��	Password:��$��Center=�$��Center=�$��14(�� $��1��.+�G� -    G�Name$ txtPassword��$��Center=�
���   �$��30��$��14(�$��Center=�{�.  � $��3��$��30��$��0,5,0,0q� �� $��* ��� $��* �.+�� $��0��
�������$��0,0,5,0q�.+��� -   $ 
btnOKLogin��#���   $��OK��$��14(� �� 	IsDefault .  J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect  � $��1��.+��� -   $ btnCloseLogin��#���   $��Close��$��14(�   