-- ---
-- TEST DATA
-- inserts test user data into db
-- ---

INSERT INTO `users`
    (
        `email`,
        `password`,
        `first_name`,
        `last_name`,
        `admin`
    )
    VALUES
    (
		'admin',
        MD5('admin'),
        'Admin',
        'User',
        1
    ),
    (
        'user1',
        MD5('user1'),
        'User',
        '1',
        0
    ),
    (
        'user2',
        MD5('user2'),
        'User',
        '2',
        0
    ),
    (
        'user3',
        MD5('user3'),
        'User',
        '3',
        0
    );

INSERT INTO `blocks`
	(
		`previous_hash`,
        `created_by`,
        `data`,
        `hash`,
        `nonce`,
        `isValid`
    )
    VALUES
    (
		'',
        1,
        100,
        'f090074f500f037f42e1176a271644cad5013faa31ea0232828ed693f27bf96f',
        41,
        1
    );

INSERT INTO `chains`
	(
		`name`,
        `created_by`
    ) VALUES
    ('test', 1),
    ('test_two_chains', 1);

-- Inserting user-chain access
INSERT INTO `block_chain_user`
	(
		`chain_id`,
        `user_id`,
        `block`
    ) VALUES
    -- (1, 1, 0),
    (1, 2, 0),
    (1, 3, 0),
    (1, 4, 0),
    (2, 2, 0),
    (2, 3, 0),
    (2, 4, 0);

-- Inserting block-chain relationship
INSERT INTO `block_chain_user`
	(
		`chain_id`,
        `block_id`,
        `block`
    ) VALUES
    (1, 1, 1),
    (2, 1, 1);
    
-- Inserting group
INSERT INTO `groups` (`name`) VALUES ("test users"), ("test second group");

-- Inserting user_group relationship
INSERT INTO `users_groups` 
(`user_id`, `group_id`) VALUES
(2, 1),
(3, 1),
(4, 1),
(2, 2),
(4, 2);
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    