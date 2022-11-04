import React from "react";
import styled from "styled-components";

const ProfilePicture = styled.img`
  border-radius: 15px;

  width: ${props => props.isComment ? '3em' : '4em'};
  height: ${props => props.isComment ? '3em' : '4em'};
`

class ProfilePictureComponent extends React.Component{
    render(){
        return(
            <ProfilePicture
                isComment={this.props.isComment}
                className={this.props.className}
                src="http://placekitten.com/256/256"/>
        )
    }
}

export default ProfilePictureComponent;