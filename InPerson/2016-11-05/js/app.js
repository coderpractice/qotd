/*Write a function that returns true if the given Binary Tree is SumTree else false. A SumTree is a Binary Tree where the value of a node is equal to sum of the nodes present in its left subtree and right subtree. An empty tree is SumTree and sum of an empty tree can be considered as 0. A leaf node is also considered as SumTree.*/
var tree = [26,10,3,4,6,3];
function isSumTree(tree) {
   return isSumTreeInternal(tree, 0);
}
function isSumTreeInternal(tree, nodeIndex) {
    if(nodeIndex >= tree.length) {
        return {
            val:0,
            status: true
        }
    }

    let leftIndex = (nodeIndex * 2) + 1;
    let rightIndex = (nodeIndex * 2) + 2;
    let status = true;
    if(leftIndex < tree.length) {
       let left = isSumTreeInternal(tree, leftIndex);
       let right = isSumTreeInternal(tree, rightIndex);
       return {
                val: tree[nodeIndex] + left.val + right.val,
                status: tree[nodeIndex] === left.val + right.val && left.status && right.status
            };

    }
        return {
            val: tree[nodeIndex],
            status: true
        };
}

function Num(x)  {
    return x ||0;
}
console.log(isSumTree(tree));