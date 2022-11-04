import React from 'react';
import styled from 'styled-components';

const Container = styled.div`
  display: flex;
  justify-content: center;
  width: 100%;
  height: 100%;
`;

const ContentContainer = styled.div`
  // This container limits the content to a smaller area in the center of the viewport.
  width: calc(100% - 1em);
  height: 100%;

  @media (min-width: 60em) {
    width: 58em;
  }
`;

class MainContentArea extends React.Component {
    render() {
        return (
            <Container>
                <ContentContainer>
                    {this.props.children}
                </ContentContainer>
            </Container>
        );
    }
}

export default MainContentArea;