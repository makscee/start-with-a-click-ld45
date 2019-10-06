using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class Collection : MonoBehaviour
{
    private class Node
    {
        public int value, lvl;
        public Node[] children = new Node[4];
        public Vector2 position;
        public float size;
        public Color color;
        public GameObject dot;

        public Node(int value)
        {
            this.value = value;
        }
    }

    public Color[] lvlColors;
    
    private const float padding = 3f, size = 8f;
    public GameObject collectionDot;
    public BottomScreen bottomScreen;
    private List<GameObject> _dots = new List<GameObject>();

    private void OnEnable()
    {
        CreateStructure();
//        AddValue(255);
    }

    private Node _root;
    private void CreateStructure()
    {
        int l = 16;
        _root = new Node(l * l)
        {
            position = new Vector2(size * l / 2 + padding * (l / 2 + 0.5f), -(size * l / 2 + padding * (l / 2 + 0.5f))),
            size = (size * l + padding * (l - 1) + 2),
            color = lvlColors[4],
            lvl = 5,
        };
        CreateChildren(_root);
    }

    private void CreateChildren(Node node, int lvl = 3)
    {
        if (node.value == 1) return;
        var cSize = (node.size - 2) / 2 - 2;
        var pSize = node.size / 4;
        float px = node.position.x, py = node.position.y;
        var c = lvlColors[lvl];
        var positions = new[]
        {
            new Vector2(px - pSize, py + pSize), 
            new Vector2(px + pSize, py + pSize), 
            new Vector2(px - pSize, py - pSize), 
            new Vector2(px + pSize, py - pSize), 
        };
        for (var i = 0; i < 4; i++)
        {
            var child = new Node(node.value / 4)
            {
                size = cSize,
                position = positions[i],
                color = c,
                lvl = lvl + 1,
            };
            node.children[i] = child;
            CreateChildren(child, lvl - 1);
        }
    }

    private void CreateDot(Node node)
    {
        var dot = Instantiate(collectionDot, bottomScreen.transform);
        var rect = dot.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(node.position.x, node.position.y);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, node.size);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, node.size);
        dot.GetComponent<RawImage>().color = node.color;
        node.dot = dot;
        var colDot = dot.GetComponent<CollectionDot>();
        colDot.value = node.value;
        colDot.lvl = node.lvl;
        colDot.attachedTo = bottomScreen;
    }

    private int curValue = 0;
    public void AddValue(int value)
    {
        curValue += value;
        var recValue = curValue;
        CleanDots(_root);
        RecreateDots(_root, ref recValue);
    }

    private static void CleanDots(Node node)
    {
        Destroy(node.dot);
        if (node.value == 1) return;
        foreach (var child in node.children)
        {
            CleanDots(child);
        }
    }

    public void DetachDot(GameObject dot)
    {
        DetachSearch(_root, dot);
    }

    private static void DetachSearch(Node node, Object dot)
    {
        if (node.dot == dot)
        {
            node.dot = null;
            return;
        }

        if (node.value == 1) return;
        foreach (var child in node.children)
        {
            DetachSearch(child, dot);
        }
    }
    private void RecreateDots(Node node, ref int value)
    {
        if (value == 0)
        {
            return;
        }
        if (node.value <= value)
        {
            value -= node.value;
            CreateDot(node);
            return;
        }
        
        foreach (var child in node.children)
        {
            RecreateDots(child, ref value);
        }
    }
}