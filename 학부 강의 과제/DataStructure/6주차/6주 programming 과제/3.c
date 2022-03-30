#include <stdio.h>
#include <stdlib.h>
#include <conio.h>
#include <malloc.h>
struct node
{
	int data;
	struct node *next;
};
struct node *start = NULL;
struct node *create_ll(struct node *);
struct node *display(struct node *);
struct node *multiply(struct node *);

int main(int argc, char *argv[]) {
	int option;

	start = create_ll(start);
	printf("\n");
	start =  display(start);
	printf("\n");
	start = multiply(start);
	_getch();
	return 0;
}
struct node *create_ll(struct node *start)
{
	struct node *new_node, *ptr;
	int num;
	printf("\n end = 0");
	printf("\n input : ");
	scanf("%d", &num);
	while (num != 0)
	{
		new_node = (struct node*)malloc(sizeof(struct node));
		new_node->data = num;
		if (start == NULL)
		{
			new_node->next = NULL;
			start = new_node;
		}
		else
		{
			ptr = start;
			while (ptr->next != NULL)
				ptr = ptr->next;
			ptr->next = new_node;
			new_node->next = NULL;
		}
		printf("\n input : ");
		scanf("%d", &num);
	}
	return start;
}
struct node *display(struct node *start)
{
	struct node *ptr;
	ptr = start;
	while (ptr != NULL)
	{
		printf("\t %d", ptr->data);
		ptr = ptr->next;
	}
	return start;
}
struct node *multiply(struct node *start) {
	struct node *ptr;
	ptr = start;
	while (ptr != NULL)
	{
		printf("\t %d", (ptr->data) * 10);
		ptr = ptr->next;
	}
	return start;
}