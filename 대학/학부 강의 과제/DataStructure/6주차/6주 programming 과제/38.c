#include <stdio.h>
#include <stdlib.h>


/* Basic node structure. */
struct node {
	int data;
	struct node * next;
};


/* Functions declarations */
void createList(struct node ** head, int n);
void displayList(struct node ** head);
void deleteAll(struct node ** head, int key);


int main()
{
	int n, key, data, choice;

	struct node * head = NULL;

	printf("Node Number of Generations : ");
	scanf("%d", &n);
	createList(&head, n);
	printf("node select : ");
	scanf("%d", &key);
	deleteAll(&head, key);
	displayList(&head);

}


/**
 * Delete all occurrence of an element by key from a
 * given circular linked list.
 */
void deleteAll(struct node ** head, int key)
{
	int i, count;
	struct node *prev, *cur;

	if (*head == NULL)
	{
		printf("List is empty.\n");
		return;
	}

	count = 0;
	cur = *head;
	prev = cur;


	// Find node before head node
	while (prev->next != *head)
	{
		prev = prev->next;
		count++;
	}

	// Iterate till first node
	i = 0;
	while (i <= count)
	{
		if (cur->data == key)
		{
			// Link prev node with next node of current
			if (cur->next != cur)
				prev->next = cur->next;
			else
				prev->next = NULL;

			// Adjust head node if needed
			if (cur == *head)
				*head = prev->next;

			// Delete current node
			free(cur);

			// Move current node ahead
			if (prev != NULL)
				cur = prev->next;
			else
				cur = NULL;
		}
		else
		{
			prev = cur;
			cur = cur->next;
		}


		i++;

	}
}


/**
 * Create a circular linked list of n nodes.
 */
void createList(struct node ** head, int n)
{
	int i, data;
	struct node *prevNode, *newNode;

	prevNode = NULL;

	/* Creates and link n nodes */
	for (i = 1; i <= n; i++)
	{
		newNode = malloc(sizeof(struct node));

		printf("input %d node: ", i);
		scanf("%d", &data);

		newNode->data = data;
		newNode->next = NULL;

		// Link the previous node with newly created node
		if (prevNode != NULL)
			prevNode->next = newNode;

		// Adjust head node 
		if (*head == NULL)
			*head = newNode;

		// Move previous node ahead
		prevNode = newNode;
	}

	// Link last node with first node
	prevNode->next = *head;

}


/**
 * Display content of the linked list.
 */
void displayList(struct node ** head)
{
	struct node *current;
	int n = 1;

	if (*head == NULL)
	{
		printf("List is empty.\n");
		return;
	}

	current = *head;
	printf("LIST : \n");

	do {
		printf("%d ", current->data);

		current = current->next;
		n++;
	} while (current != *head);
}