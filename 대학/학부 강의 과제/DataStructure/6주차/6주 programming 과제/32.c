#include <stdio.h>
#include <stdlib.h>

/* Link list node */
struct Node {
    int data;
    struct Node* next;
};

/* Function to get the middle and set at
   beginning of the linked list*/
void setMiddleHead(struct Node** head)
{
    if (*head == NULL)
        return;

    // To traverse list nodes one by one
    struct Node* one_node = (*head);

    // To traverse list nodes by skipping
    // one.
    struct Node* two_node = (*head);

    // To keep track of previous of middle
    struct Node* prev = NULL;
    while (two_node != NULL && two_node->next != NULL) {

        /* for previous node of middle node */
        prev = one_node;

        /* move one node each time*/
        two_node = two_node->next->next;

        /* move two node each time*/
        one_node = one_node->next;
    }

    /* set middle node at head */
    prev->next = prev->next->next;
    one_node->next = (*head);
    (*head) = one_node;
}

// To insert a node at the beginning of linked
// list.
void push(struct Node** head_ref, int new_data)
{
    /* allocate node */
    struct Node* new_node =
        (struct Node*)malloc(sizeof(struct Node));

    new_node->data = new_data;

    /* link the old list off the new node */
    new_node->next = (*head_ref);

    /* move the head to point to the new node */
    (*head_ref) = new_node;
}

// A  function to print a given linked list
void printList(struct Node* ptr)
{
    while (ptr != NULL) {
        printf("%d ", ptr->data);
        ptr = ptr->next;
    }
    printf("\n");
}

/* Driver function*/
int main()
{
    // Create a list of 5 nodes
    struct Node* head = NULL;
    int i;
    for (i = 5; i > 0; i--)
        push(&head, i);

    printf(" list before: ");
    printList(head);

    setMiddleHead(&head);

    printf(" list After:  ");
    printList(head);

    return 0;
}
