import React, { useEffect, useState } from 'react';
import { Card, CardContent, Typography, Button, TextField, Checkbox, FormControlLabel } from '@mui/material';

function UsersPage() {
    const [users, setUsers] = useState([]);
    const [searchQuery, setSearchQuery] = useState('');
    const [newUserEmail, setNewUserEmail] = useState('');

    useEffect(() => {
        fetch('https://localhost:7207/api/Users')
            .then(response => response.json())
            .then(data => {
                const userPromises = data.map(user =>
                    fetch(`https://localhost:7207/api/Users/id/${user.id}`)
                        .then(response => response.json())
                        .then(userData => ({ ...user, isAdmin: userData.isAdmin }))
                );
                Promise.all(userPromises).then(usersWithAdminStatus => setUsers(usersWithAdminStatus));
            })
            .catch(error => console.error('Error fetching users:', error));
    }, []);

    const deleteUser = (userId) => {
        if (window.confirm('Er du sikker på at du vil slette brugeren?')) {
            fetch(`https://localhost:7207/api/Users/${userId}`, {
                method: 'DELETE',
            })
                .then(response => {
                    if (response.ok) {
                        setUsers(users.filter(user => user.id !== userId));
                    } else {
                        console.error('Error deleting user:', response.statusText);
                    }
                })
                .catch(error => console.error('Error deleting user:', error));
        }
    };

    const handleAdminChange = (userId, isAdmin) => {
        const url = `https://localhost:7207/api/Users/${isAdmin ? 'remove-admin' : 'make-admin'}/${userId}`;
        fetch(url, {
            method: 'PUT',
        })
            .then(response => {
                if (response.ok) {
                    setUsers(users.map(user =>
                        user.id === userId ? { ...user, isAdmin: !isAdmin } : user
                    ));
                } else {
                    console.error('Error updating user admin status:', response.statusText);
                }
            })
            .catch(error => console.error('Error updating user admin status:', error));
    };

    const generateRandomPassword = () => {
        return Math.random().toString(36).slice(-8);
    };

    const createUser = () => {
        const password = generateRandomPassword();
        fetch('https://localhost:7207/api/Auth/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'accept': 'text/plain'
            },
            body: JSON.stringify({ email: newUserEmail, password })
        })
            .then(response => response.json())
            .then(newUser => {
                setUsers([...users, newUser]);
                setNewUserEmail('');
                sendEmail(newUserEmail, password);
            })
            .catch(error => console.error('Error creating user:', error));
    };

    const sendEmail = (email, password) => {
        fetch('https://localhost:7207/Mail', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'accept': 'text/plain'
            },
            body: JSON.stringify({
                emailToId: email,
                emailToName: email,
                emailSubject: 'Welcome to Hotel None Inclusive',
                emailBody: `Dear customer,\n\nThank you for registering with us! Here is your password: ${password}\n\nPlease keep it safe.\n\nBest regards,\nHotel None Inclusive`
            })
        })
            .then(response => response.json())
            .then(result => {
                if (result) {
                    console.log('Email sent successfully');
                } else {
                    console.error('Error sending email');
                }
            })
            .catch(error => console.error('Error sending email:', error));
    };

    const filteredUsers = users.filter(user =>
        user.email.toLowerCase().includes(searchQuery.toLowerCase())
    );

    return (
        <div className="users-container">
            <h1>Brugere</h1>
            <TextField
                label="Ny bruger email"
                variant="outlined"
                fullWidth
                margin="normal"
                value={newUserEmail}
                onChange={(e) => setNewUserEmail(e.target.value)}
            />
            <Button
                variant="contained"
                color="primary"
                onClick={createUser}
            >
                Opret bruger
            </Button>
            <TextField
                label="Søg med email"
                variant="outlined"
                fullWidth
                margin="normal"
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
            />
            <div className="users-list">
                {filteredUsers.map(user => (
                    <Card key={user.id} sx={{marginBottom: 2}}>
                        <CardContent>
                            <Typography variant="h5" component="div">
                                {user.firstName} {user.lastName}
                            </Typography>
                            <Typography variant="body2" color="text.secondary">
                                Email: {user.email}
                            </Typography>
                            <FormControlLabel
                                control={
                                    <Checkbox
                                        checked={user.isAdmin}
                                        onChange={() => handleAdminChange(user.id, user.isAdmin)}
                                    />
                                }
                                label="Admin"
                            />
                            <Button
                                variant="contained"
                                color="secondary"
                                onClick={() => deleteUser(user.id)}
                            >
                                Slet
                            </Button>
                        </CardContent>
                    </Card>
                ))}
            </div>
        </div>
    );
}

export default UsersPage;
