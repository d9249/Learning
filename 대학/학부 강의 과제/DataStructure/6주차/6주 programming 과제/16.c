#include<stdio.h>
#include<conio.h>

struct node
{
   int data;
   struct node *link;
}*start;
void create(int);
void disp();
void main()
{
   int ch, n, i, k, r = 0, m;
   start = NULL;
   do
   {
      printf("\n1.CREATE\n");
      printf("\n2.DISPLAY\n");
      printf("\n3.EXIT\n");
      printf("\nENTER UR CHOICE : \n");
      scanf("%d", &ch);
      switch (ch)
      {
      case 1:
         printf("\n\nENTER THE NUMBER\n");
         scanf("%d", &n);
         while (n>0)
         {
            k = n % 10;
            n = n / 10;
            r = r * 10 + k;
         }
         while (r>0)
         {
            k = r % 10;
            create(k);
            r = r / 10;
         }

         break;
      case 2:
         disp();
         break;
      case 3:
         exit(0);
      }
   } while (ch != 4);
   _getch();
}
void create(int data)
{
   struct node *q, *tmp;
   tmp = (struct node *)malloc(sizeof(struct node));
   tmp->data = data;
   tmp->link = NULL;
   if (start == NULL)
   {
      start = tmp;
   }
   else
   {
      q = start;
      while (q->link != NULL)
         q = q->link;
      q->link = tmp;
   }
}
void disp()
{
   struct node *q;
   if (start == NULL)
   {
      printf("\n\nLIST IS EMPTY");
   }
   else
   {
      q = start;
      while (q != NULL)
      {
         printf("%d->", q->data);
         q = q->link;
      }
      printf("NULL");
   }
}
void addbeg(int data)
{
   struct node *tmp;
   tmp = (struct node *)malloc(sizeof(struct node));
   tmp->data = data;
   tmp->link = start;
   start = tmp;
}
void addlast(int data)
{
   struct node *q, *tmp;
   tmp = (struct node *) malloc(sizeof(struct node));
   tmp->data = data;
   tmp->link = NULL;
   q = start;
   while (q->link != NULL)
      q = q->link;
   q->link = tmp;
}
void addpos(int data, int pos)
{
   int i;
   struct node *q, *tmp;
   tmp = (struct node *) malloc(sizeof(struct node));
   tmp->data = data;
   q = start;

   for (i = 0; i<pos - 1; i++)
   {
      q = q->link;
   }

   tmp->link = q->link;
   q->link = tmp;
}