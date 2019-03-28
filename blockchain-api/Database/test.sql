use Blockchain; select * from pending_blocks;
use Blockchain; select * from users;
use Blockchain; select * from `groups`;
use Blockchain; select * from users_groups;
use Blockchain; select * from chains;
use Blockchain; select * from blocks;
use Blockchain; select * from block_chain_user where block=0;
use Blockchain; SELECT u.first_name as fname, u.last_name as lname, b.*, bcu.chain_id FROM `blocks` AS b 
                	            INNER JOIN users AS u ON b.created_by=u.id 
                	            INNER JOIN block_chain_user AS bcu ON bcu.block_id=b.id 
                	            WHERE chain_id=1 AND b.isValid=1;
SELECT pb.*, b.isValid AS blockValid FROM `pending_blocks` AS pb INNER JOIN `blocks` as b ON b.id=pb.id WHERE b.isValid IS NULL AND pb.isValid IS NULL;
SELECT b.* FROM `blocks` as b INNER JOIN `pending_blocks` AS pb ON b.id=pb.id WHERE b.isValid IS NULL AND pb.isValid IS NULL;
SELECT g.name FROM `groups` g INNER JOIN `users_groups` ug ON ug.group_id=g.id WHERE ug.user_id=2;
SELECT u.id, u.email, u.first_name, u.last_name FROM `users` u
 INNER JOIN `users_groups` ug ON u.id=ug.user_id
 WHERE group_id=2 and u.admin=0 and u.id!=1;