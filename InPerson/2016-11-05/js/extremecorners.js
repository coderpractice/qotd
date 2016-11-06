//Given a binary tree, print nodes of extreme corners of each level but in alternate order.
//Assumptions: This is a complete binary tree
var tree = [1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34];
function printExtremeBounds(tree){
    if(!tree || tree.length === 0){
        return;
    }
    var output=[];
    output.push(tree[0]);
    var left = 1;
    var right = 2;
    var flag = true;
    while(right < tree.length || (flag && left < tree.length)) {
        if(flag) {
          output.push(tree[left]);   
        }
        else {
          output.push(tree[right]);   
        }
        left = (left * 2) +1;
        right = (right *2)  + 2;
        flag = !flag;
    }
    console.log(output);
}

printExtremeBounds(tree);