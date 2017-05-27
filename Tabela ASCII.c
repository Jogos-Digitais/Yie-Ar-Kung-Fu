#include <stdio.h>

//Tabela ASCII | Caracteres 0 - 255
void main()
{
 int i;
 
 for(i = 0; i < 256; i++)
 printf("%d | %c\n", i, i);
 system("pause");
}
