#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <malloc.h>
struct node
{
	int data;
	struct node *next;
};
struct node *start = NULL;
struct node *create_ll(struct node *);
struct node *display(struct node *);
bool isPairWiseSorted(struct Node* head);
int main() {
	int option;
	do
	{
		printf("\n\n *****MAIN MENU *****");
		printf("\n 1: Create a list");
		printf("\n 2: Display the list");
		printf("\n 3: issort?");
		printf("\n 13: EXIT");
		printf("\n\n Enter your option : ");
		scanf("%d", &option);
		switch (option)
		{
		case 1: start = create_ll(start);
			printf("\n LINKED LIST CREATED");
			break;
		case 2: start = display(start);
			break;
		case 3: 
			if (isPairWiseSorted(start))
				printf("1");
			else
				printf("0");
		}
	} while (option != 13);
	_getch();
	return 0;
}
struct node *create_ll(struct node *start)
{
	struct node *new_node, *ptr;
	int num;
	printf("\n Enter - 1 to end");
	printf("\n Enter the data : ");
	scanf("%d", &num);
	while (num != -1)
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
		printf("\n Enter the data : ");
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
	printf("\n");
	return start;
}

bool isPairWiseSorted(struct node* start)
{
	bool flag = true;
	struct node* temp = start;
	while (temp != NULL && temp->next != NULL) {
		if (temp->data > temp->next->data) {
			flag = false;
			break;
		}
		temp = temp->next->next;
	}
	return flag;
}