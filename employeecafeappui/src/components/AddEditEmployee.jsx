import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { RadioGroup, FormControlLabel, Box, Radio, Button, MenuItem, Select, InputLabel,FormLabel, FormControl, Dialog, DialogActions, DialogContent, DialogTitle } from '@mui/material';
import TextBox from './TextBox';

const AddEditEmployee = ({ onClose, employeeID, onSubmit }) => {
    const [employee, setEmployee] = useState({
        name: '',
        emailAddress: '',
        phoneNumber: '',
        gender: '',
        cafeID: ''
    });
    const [cafes, setCafes] = useState([]);
    const [isDirty, setIsDirty] = useState(false);

    useEffect(() => {
        // Fetch cafes for dropdown
        axios.get('https://localhost:7032/api/Cafe')
            .then(response => {
                setCafes(response.data);
            })
            .catch(error => {
                console.error("There was an error fetching the cafes:", error);
            });

        if (employeeID) {
            axios.get(`https://localhost:7032/api/Employee`)
                .then(response => {
                    const selectedEmp = response.data.find(employee => employee.employeeID === employeeID);
                    if (selectedEmp) {
                        setEmployee(selectedEmp);
                    }
                })
                .catch(error => {
                    console.error("There was an error fetching the employee:", error);
                });
        }
    }, [employeeID]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setEmployee({ ...employee, [name]: value });
        setIsDirty(true);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (employeeID) {
            await updateEmployee(employeeID, employee);
        } else {
            await addEmployee(employee);
        }
        onSubmit();
        onClose();
    };

    const addEmployee = (employee) => {
        const formData = new FormData();
        formData.append('name', employee.name);
        formData.append('email_Address', employee.emailAddress);
        formData.append('phonenumber', employee.phoneNumber);
        formData.append('gender', employee.gender);
        formData.append('cafeID', employee.cafeID);
        
        console.log('Adding new employee with data:', formData);
        try {
            const response = axios.post('https://localhost:7032/api/Employee', formData, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            console.log('Employee added successfully:', response.data);
        } catch (err) {
            console.error('Error adding employee:', err.response ? err.response.data : err.message);
        }
    };

    const updateEmployee = async (employeeId, employee) => {
        const data = {
            employeeID: employeeId,
            name: employee.name,
            email_Address: employee.emailAddress,
            phonenumber: employee.phoneNumber,
            gender: employee.gender,
            cafeID: employee.cafeID
        };
        console.log('Updating employee with data:', data);
        try {
            const response = await axios.put(`https://localhost:7032/api/Employee?employeeID=${employeeId}`, data, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            console.log('Employee updated successfully:', response.data);
        } catch (err) {
            console.error('Error updating employee:', err.response ? err.response.data : err.message);
        }
    };

    const handleCancel = () => {
        if (isDirty && !window.confirm('You have unsaved changes. Are you sure you want to leave?')) {
            return;
        }
        onClose();
    };

    return (
        <Dialog open onClose={handleCancel} maxWidth="sm" fullWidth>
            <DialogTitle><h2>{employeeID ? 'Edit Employee' : 'Add Employee'}</h2></DialogTitle>
            <DialogContent>
                <form onSubmit={handleSubmit}>
                    <Box display="flex" alignItems="center" mb={1}>
                      <FormLabel sx={{ mr: 8.5 }}>Name:</FormLabel>
                        <TextBox
                            value={employee.name}
                            onChange={handleChange}
                            name="name"
                            minLength={6}
                            maxLength={10}
                            required
                         />
                    </Box>
                    <Box display="flex" alignItems="center" mb={1}>
                        <FormLabel sx={{ mr: 9 }}>Email:</FormLabel>
                        <TextBox
                            value={employee.emailAddress}
                            onChange={handleChange}
                            name="emailAddress"
                            type="email"
                            required
                        />
                    </Box>
                    <Box display="flex" alignItems="center" mb={1}>
                        <FormLabel sx={{ mr: 1 }}>PhoneNumber:</FormLabel>
                        <TextBox
                            value={employee.phoneNumber}
                            onChange={handleChange}
                            name="phoneNumber"
                            pattern="^[89]\d{7}$"
                            required
                            />
                    </Box>
                    <Box display="flex" alignItems="center" mb={0}>
                        <FormLabel sx={{ mr: 7 }}>Gender:</FormLabel>
                        <RadioGroup
                            row
                            name="gender"
                            value={employee.gender}
                            onChange={handleChange}>
                            <FormControlLabel value="Male" control={<Radio />} label="Male" />
                            <FormControlLabel value="Female" control={<Radio />} label="Female" />
                        </RadioGroup>
                    </Box>
                    <Box display="flex" alignItems="center" mt={0}>
                    <FormControl sx={{ width: '50%' }} margin="normal">
                        <FormLabel id="cafe-label">Assigned Cafe:</FormLabel>
                        <Select
                            name="cafeID"
                            value={employee.cafeID}
                            onChange={handleChange}
                        >
                            {cafes.map(cafe => (
                                <MenuItem key={cafe.id} value={cafe.id}>
                                    {cafe.name}
                                </MenuItem>
                            ))}
                        </Select>
                        </FormControl>
                    </Box>
                </form>

            </DialogContent>
            <DialogActions>
                <Button onClick={handleSubmit} color="primary">Submit</Button>
                <Button onClick={handleCancel} color="secondary">Cancel</Button>
            </DialogActions>
        </Dialog>
    );
};


export default AddEditEmployee;