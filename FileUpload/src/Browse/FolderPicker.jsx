import React, {Component, PropTypes} from "react";
import ReactDOM from "react-dom";
import Folders from "./Folders";
import { Scrollbars } from "react-custom-scrollbars";
const searchIcon = require("!raw!./img/search.svg");

export default class FolderPicker extends Component {

    constructor() {
        super();
        this.state = {
            showFolderPicker: false,
            searchFolderText: ""
        };
        this.handleClick = this.handleClick.bind(this);
    }

    componentDidMount() {
        document.addEventListener("click", this.handleClick, false);
        this._isMounted = true;
    }

    componentWillUnmount() {
        document.removeEventListener("click", this.handleClick, false);
        this._isMounted = false;
    }

    handleClick(e) {
        if (!this._isMounted) { return; }
        const node = ReactDOM.findDOMNode(this);
        if (node && node.contains(e.target)) {
            return;
        }
        this.hide();
    }

    hide() {
        this.setState({ showFolderPicker: false, searchFolderText: "" });
    }
    onFolderClick(folder) {
        this.hide();
        this.props.onFolderClick(folder);
    }

    onChangeSearchFolderText(e) {
        const searchFolderText = e.target.value;
        this.setState({ searchFolderText });
    }

    onFoldersClick() {
        const {showFolderPicker} = this.state;
        this.setState({ showFolderPicker: !showFolderPicker });
    }

    render() {
        /* eslint-disable react/no-danger */
        const selectedFolderText = this.props.selectedFolder ? this.props.selectedFolder.value : "<Not Specified>";

        return <div className="drop-down">
            <div className="selected-item" onClick={this.onFoldersClick.bind(this) }>
                {selectedFolderText}
            </div>
            <div className={"item-picker-container" + (this.state.showFolderPicker ? " show" : "") } >
                <div className="inner-box">
                    <div className="search">
                        <input type="text" value={this.state.searchFolderText} onChange={this.onChangeSearchFolderText.bind(this) } placeholder="Search Folders..." />
                        <div className="search-icon" dangerouslySetInnerHTML={{ __html: searchIcon }} />
                    </div>
                    <div className="items">
                        <Scrollbars className="scrollArea content-vertical"
                            autoHeight
                            autoHeightMin={0}
                            autoHeightMax={200}>
                            <Folders
                                searchFolderText={this.state.searchFolderText}
                                folders={this.props.folders}
                                getChildren={this.props.getChildren}
                                onFolderClick={this.onFolderClick.bind(this) }/>
                        </Scrollbars>
                    </div>
                </div>
            </div>
        </div>;
    }
}


FolderPicker.propTypes = {
    folders: PropTypes.object.isRequired,
    onFolderClick: PropTypes.func.isRequired,
    getChildren: PropTypes.func.isRequired,
    selectedFolder: PropTypes.object.isRequired
};
