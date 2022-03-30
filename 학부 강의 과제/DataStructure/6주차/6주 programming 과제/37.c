// C program for a Header Linked List 
#include <malloc.h> 
#include <stdio.h> 

// Structure of the list 
struct link {
	int info;
	struct link* next;
};

// Empty List 
struct link* start = NULL;

// Function to create a header linked list 
struct link* create_header_list(int data)
{

	// Create a new node 
	struct link *new_node, *node;
	new_node = (struct link*)
		malloc(sizeof(struct link));
	new_node->info = data;
	new_node->next = NULL;

	// If it is the first node 
	if (start == NULL) {

		// Initialize the start 
		start = (struct link*)
			malloc(sizeof(struct link));
		start->next = new_node;
	}
	else {

		// Insert the node in the end 
		node = start;
		while (node->next != NULL) {
			node = node->next;
		}
		node->next = new_node;
	}
	return start;
}

// Function to display the 
// header linked list 
struct link* display()
{
	struct link* node;
	node = start;
	node = node->next;
	while (node != NULL) {
		printf("%d ", node->info);
		node = node->next;
	}
	printf("\n");
	return start;
}

int main()
{

	create_header_list(11);
	create_header_list(12);
	create_header_list(13);
	display();

	create_header_list(14);
	create_header_list(15);
	display();

	return 0;
}
