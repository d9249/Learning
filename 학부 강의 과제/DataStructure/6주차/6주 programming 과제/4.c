#include<stdio.h>
#include<conio.h>
struct node
{
	int data;
	struct node *link;
}*start;

void create(int);
void disp();
void count();
void main()
{
	int ch, n, i, m, a, pos;
	system("cls");
	start = NULL;
	
	printf("nodes crarte Number :");
	scanf("%d", &n);
	for (i = 0; i < n; i++)
	{
		printf("input : ");
		scanf("%d", &m);
		create(m);
	}
	count();
	getch();
}
void count()
{
	struct node *q;
	int nonz = 0;
	q = start;
	while (q != NULL)
	{
		if (q->data > 0)
		{
			nonz++;
		}


		q = q->link;
	}
	printf("\n Non-zero elements = %d", nonz);
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
		printf("\n Empty List ");
	}
	else
	{
		q = start;
		while (q != NULL)
		{
			printf(" %d -> ", q->data);
			q = q->link;
		}
		printf("NULL");
	}
}

