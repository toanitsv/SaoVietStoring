   M S B A M L   `   `   `  ����  SaoVietStoring    SaoVietStoring.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Storing System��5   	   $	��400��6    $	��700��6-    � 
ResizeMode$ CanMinimize=�69    � WindowStyle$ SingleBorderWindow=�6i    � WindowStartupLocation$ CenterScreen=�6�    � 	Resources 5         RoundCorner �� 
TargetType �� Property �� Value �� 
TargetType	 �Name
 ��Name �� CornerRadius � Offset � Color �� Triggers P� Property P� Value �� 
TargetName  LinearBackground �� 
StartPoint �� EndPoint��%2  &	        &	 |    �� 5   
   " ��6$   .�5      ��  � �� 5      �� 5	      " ��6&   .R�5
      � $
	 grid��6   .+�5      �� $
 border��6%   $ 0,0,0,0��65   
���   �$��1q�6`   ��5   "   
� 5   &   .��5   *   � $	 0.3Q�67   $ Bisque��6D   5   '   ._�5   "   �� $��Center=�63   $��Center=�5   +   5   #   5      5       5      P� 5       � �� . . �5   "   �� $ border��6?    � �� 5   &   
� 5   *   .��5   .   � $ 	LightBlue��6;   $	 0.3Q�6M   5   +   5   #   5      P� 5        �@J�IsMouseOver . . �5!   "   �� $ border��6@    � �� �#F00F6T   �� 5"   "   $ border��6D    � ��$ 2q�6X   �� 5#   "   $ border��6?    � �� 5$   &   
� 5%   *   .��5&   .   � $ LightSeaGreen��6;   $	 0.3Q�6Q   5'   +   5)   #   5*      5,      5.      5/      �� 50   
   $	 0,1&�67   $	 1,0&�6H   ��52      � $
 -0.5Q�6   $ LightSeaGreen��6-   � 53      $ 1Q�6   $ Transparent��6*   55      56      .��57      �  � RowDefinitions 58   
   �� 59      $
��auto �6   �� 5:      .+�5<   
   � $��0��6    � ColumnDefinitions 5=      �� 5>      �� 5?      $
��auto �6#   .+�5A      o� $��0��6   #��[ 6    $��Caribi)�6O   $��14(�6c   $��Center=�6q   $��Center=�6�   .@�5B      m� $��Tools��6   .@�5C      m� -   5         m�Name$ miInput��5C      {�.  $��Input��6B   6Q   m� 5D      -   5        $ miOutput��5D      {�.  $��Output��6D   6T   m� 5E      -   5        $ miReWeighCarton��5E      {�.  $��Re-Weigh Carton��6J   6c   m� 5F      -   5        $ miImportPORepacking��5F      {�.  $��Import PO Repacking��6N   6k   5G      m� 5H      $��Report��6   .@�5I      m� -   5        $ miInputReport��5I      $��Input Report��66   6L   m� 5J      -   5        $ miDetailReport��5J      $��Detail Report��67   6N   5K      m� 5L      $��Print��6   .@�5M      m� -   5        $ miPrintBarcode��5M      $��Barcode Temporary��67   6R   5N      m� 5O      $
��Help��6   .@�5P      m� -	   5        $ miAbout��5P      $��About me��60   6B   m� 5Q      -
   5        $ 	miVersion��5Q      $��Version��62   6C   5R      5S      �� 5T      -   5         ��Name$ 
btnReLogin��5T      $��1��6)   $��Login��69   {�.  $y�	Collapsed=�6[   
����� �5U      5V      � 5W   
   $��1��6   .+�5X      �� -   5         ��Name$ imgBackground��5X      $��Images/main.jpg��6+   $y�	Collapsed=�6D   6]   5Y      � 5Z      -   5        $	 	gridLogin��5Z       $��1��6   #��[ 63    5[      �� 5\      �� 5]      $
��auto �6    �� 5^       5`      �� 5a      �� 5b      $
��auto �6#   �� 5c      .+�5e      �� -   5        $
 brLogin��5e      $��1��6&   $��1��63   $	��380��6C   ._�5f      �  5g      �� 5h      $
��auto �6(   �� 5i      $
��auto �6(   �� 5j      $
��auto �6(   �� 5k      $
��auto �6(   .+�5m      � -   5        $ 	 backgroundStatusConnection��5m      $��0��6?   $��30��6L   
��� � �.+�5n      �� -   5         ��Name$ tblConnectionStatus��5n   $   $��Center=�6A   $%��--- Login to Storing System ---��6\   
���   �$��Center=�6�   $��15(�6�   $
��Bold"�6�   5o      � 5p      $��1��6   $��30��6(   $��0,5,0,0q�64    5q      �� 5r      $��* �6/   �� 5s      $��2* �6/   .+�5u      � $��0��6   
�������$��0,0,5,0q�6E   .+�5v      �� $��
User Name:��6(   $��Center=�6:   $��Center=�6U   $��14(�6r   5w      � 5x      $��1��6   .+�5y      �� -   5         ��Name$ txtUserName��5y   &   $��Center=�6;   
���   �$��30��6j   $��14(�6v   $��Center=�6�   {�.  6�   5z      5{      � 5|      $��2��6   $��30��6(   $��0,5,0,0q�64    5}      �� 5~      $��* �6/   �� 5      $��2* �6/   .+�5�      � $��0��6   
�������$��0,0,5,0q�6E   .+�5�      �� $��	Password:��6(   $��Center=�69   $��Center=�6T   $��14(�6q   5�      � 5�      $��1��6   .+�5�      G� -   5         G�Name$ txtPassword��5�   *   $��Center=�6?   
���   �$��30��6n   $��14(�6z   $��Center=�6�   {�.  6�   5�      5�      � 5�      $��3��6   $��30��6(   $��0,5,0,0q�64    5�      �� 5�      $��* �6/   �� 5�      $��* �6/   .+�5�      � $��0��6   
�������$��0,0,5,0q�6E   .+�5�      �� -   5        $ 
btnOKLogin��5�   %   #���   69   $��OK��6_   $��14(�6l    �� 	IsDefault .  J� Effect 5�   "   3  -System.Windows.Media.Effects.DropShadowEffect  5�   &   5�      5�      � 5�      $��1��6   .+�5�      �� -   5        $ btnCloseLogin��5�   %   #���   6<   $��Close��6b   $��14(�6r    5�   "     5�   &   5�      5�      5�      5�      5�      5�      5�      5�      