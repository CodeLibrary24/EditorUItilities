using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace  UIToolKitUtilities
{
    public abstract class GenericListView<T> : VisualElement where T:VisualElement
    {
        private const string _ASSET_PATH = "Assets/UIToolkitUtilities/Editor/GenericListView";
        private const string _UXML_PATH = _ASSET_PATH + "/GenericListView.uxml";

        private ListView _objectsList;
        private Button _addButton;
        private Button _removeButton;

        public event Action<object> OnSelectedItemChange;
        public event Action<int, int> OnReorder;
        public event Action OnAddItem;
        public event Action OnRemoveItem;
        
        public IList ItemSource { get; set; }
            
        protected abstract T MakeListItem();
        protected abstract void BindListItem(T item, int index);
        
        protected GenericListView()
        {
            Initialize();
            CacheReferences();
            SetupDataList();
            SetupFooter();
        }

        private void Initialize()
        {
            VisualTreeAsset uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(_UXML_PATH);
            hierarchy.Add(uxml.Instantiate());
            
            OnInitialized();
        }

        protected virtual void OnInitialized()
        {
        }

        private void CacheReferences()
        {
            _objectsList = this.Q<ListView>();
            _addButton = this.Q<Button>("add-button");
            _removeButton = this.Q<Button>("remove-button");
        }

        private void SetupDataList()
        {
            _objectsList.makeItem = MakeListItem;
            _objectsList.bindItem = (element, i) => BindListItem(element as T, i);

            _objectsList.onSelectionChange += OnSelectionChange;
            _objectsList.itemIndexChanged += OnItemIndexChanged;

            RefreshObjectsList();
        }

        private void SetupFooter()
        {
            _addButton.clicked += OnAddClicked;
            _removeButton.clicked += OnRemoveClicked;
        }
        
        private void OnSelectionChange(IEnumerable<object> selectedItems)
        {
            foreach (var obj in selectedItems)
            {
                OnSelectedItemChange?.Invoke(obj);
                break;
            }
        }

        private void OnItemIndexChanged(int first, int second)
        {
            OnReorder?.Invoke(first, second);
            RefreshObjectsList();
        }
        
        private void OnAddClicked()
        {
            OnAddItem?.Invoke();
            RefreshObjectsList();
        }

        private void OnRemoveClicked()
        {
            OnRemoveItem?.Invoke();
            RefreshObjectsList();
        }
        
        protected virtual void RefreshObjectsList()
        {
            if (ItemSource == null) return;
            
            _objectsList.itemsSource = ItemSource;
            _objectsList.RefreshItems();
        }
        
    }
}
