import React from 'react';
import {
    MDBContainer,
    MDBInput,
    MDBCheckbox,
    MDBBtn,
    MDBIcon
} from 'mdb-react-ui-kit';

import { constants } from '../../AppUrls';

class LoginComponent extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            email: '',
            password: '',
            rememberMe: true
        };
    };

    submitForm(e) {
        e.preventDefault();
        this.postData(constants.BASE_URL + "/api/account/login", {
            email: this.state.email, password: this.state.password,
            isPersistent: this.state.rememberMe
        })
            .then((data) => {
                console.log(data); // JSON data parsed by `data.json()` call
            });
    }

    async postData(url = "", data = {}) {

        var urlencoded = new URLSearchParams();
        urlencoded.append("email", data.email);
        urlencoded.append("password", data.password);
        urlencoded.append("isPersistent", data.isPersistent);

        var requestOptions = {
            method: 'POST',
            headers: {
                "Content-Type": "application/x-www-form-urlencoded"
            },
            body: urlencoded,
            redirect: 'follow'
        };

        try {
            const response = await fetch(constants.BASE_URL + "/api/account/login", requestOptions);
            const result = await response.json();
            console.log(result);
            // if (result.statusCode == 200 && result.redirectUrl) {
            //     window.location.replace(result.redirectUrl);
            // }
        } catch (error) {
            return console.log('error', error);
        }
    }

    handleInputChange(event) {
        const target = event.target;
        const name = target.name;
        const value = target.value;

        this.setState({
            [name]: value
        });
    }

    handleChecked() {
        this.setState({
            rememberMe: !this.state.rememberMe // flip boolean value
        }, () => {
            console.log(this.state);
        });
    }

    render() {
        return (

            <MDBContainer className="p-3 my-5 d-flex flex-column w-50" >
                <form onSubmit={this.submitForm.bind(this)}>
                    <MDBInput wrapperClass='mb-4' name="email" value={this.state.email} label='Email address' id='form1' type='email'
                        onChange={this.handleInputChange.bind(this)} />
                    <MDBInput wrapperClass='mb-4' name="password" label='Password' value={this.state.password} id='form2' type='password'
                        onChange={this.handleInputChange.bind(this)} />

                    <div className="d-flex justify-content-between mx-3 mb-4">
                        <MDBCheckbox name='rememberMe' checked={this.state.rememberMe} onChange={this.handleChecked.bind(this)} id='flexCheckDefault' label='Remember me' />
                        <a href="!#">Forgot password?</a>
                    </div>

                    <MDBBtn className="mb-4">Sign in</MDBBtn>

                    <div className="text-center">
                        <p>Not a member? <a href="#!">Register</a></p>
                        <p>or sign up with:</p>

                        <div className='d-flex justify-content-between mx-auto' style={{ width: '40%' }}>
                            <MDBBtn tag='a' color='none' className='m-1' style={{ color: '#1266f1' }}>
                                <MDBIcon fab icon='facebook-f' size="sm" />
                            </MDBBtn>

                            <MDBBtn tag='a' color='none' className='m-1' style={{ color: '#1266f1' }}>
                                <MDBIcon fab icon='twitter' size="sm" />
                            </MDBBtn>

                            <MDBBtn tag='a' color='none' className='m-1' style={{ color: '#1266f1' }}>
                                <MDBIcon fab icon='google' size="sm" />
                            </MDBBtn>

                            <MDBBtn tag='a' color='none' className='m-1' style={{ color: '#1266f1' }}>
                                <MDBIcon fab icon='github' size="sm" />
                            </MDBBtn>

                        </div>
                    </div>
                </form>
            </MDBContainer>
        );
    }
}

export default LoginComponent;